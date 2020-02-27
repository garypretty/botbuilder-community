﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace Bot.Builder.Community.Cards.Management.Tree
{
    internal static class CardTree
    {
        private const string SpecifyManually = " Try specifying the node type manually instead of using null.";

        private static readonly Dictionary<string, TreeNodeType> _cardTypes = new Dictionary<string, TreeNodeType>(StringComparer.OrdinalIgnoreCase)
        {
            { CardConstants.AdaptiveCardContentType, TreeNodeType.AdaptiveCard },
            { AnimationCard.ContentType, TreeNodeType.AnimationCard },
            { AudioCard.ContentType, TreeNodeType.AudioCard },
            { HeroCard.ContentType, TreeNodeType.HeroCard },
            { ReceiptCard.ContentType, TreeNodeType.ReceiptCard },
            { SigninCard.ContentType, TreeNodeType.SigninCard },
            { OAuthCard.ContentType, TreeNodeType.OAuthCard },
            { ThumbnailCard.ContentType, TreeNodeType.ThumbnailCard },
            { VideoCard.ContentType, TreeNodeType.VideoCard },
        };

        private static readonly Dictionary<TreeNodeType, ITreeNode> _tree = new Dictionary<TreeNodeType, ITreeNode>
        {
            {
                TreeNodeType.Batch, new ListTreeNode<IMessageActivity>(TreeNodeType.Activity, PayloadIdTypes.Batch)
            },
            {
                TreeNodeType.Activity, new TreeNode<IMessageActivity, IEnumerable<Attachment>>(async (activity, nextAsync) =>
                {
                    // The nextAsync return value is not needed here because the Attachments property reference will remain unchanged
                    await nextAsync(activity.Attachments, TreeNodeType.Carousel).ConfigureAwait(false);

                    return activity;
                })
            },
            {
                TreeNodeType.Carousel, new ListTreeNode<Attachment>(TreeNodeType.Attachment, PayloadIdTypes.Carousel)
            },
            {
                TreeNodeType.Attachment, new TreeNode<Attachment, object>(async (attachment, nextAsync) =>
                {
                    if (_cardTypes.ContainsKey(attachment.ContentType))
                    {
                        // The nextAsync return value is needed here because the attachment could be an Adaptive Card
                        // which would mean a new object was generated by the JObject conversion/deconversion
                        attachment.Content = await nextAsync(attachment.Content, _cardTypes[attachment.ContentType]).ConfigureAwait(false);
                    }

                    return attachment;
                })
            },
            {
                TreeNodeType.AdaptiveCard, new TreeNode<object, IEnumerable<JObject>>(async (card, nextAsync) =>
                {
                    // Return the new object after it's been converted to a JObject and back
                    // so that the attachment node can assign it back to the Content property
                    return await card.ToJObjectAndBackAsync(
                        async cardJObject =>
                        {
                            await nextAsync(
                                AdaptiveCardUtil.NonDataDescendants(cardJObject)
                                    .Select(token => token is JObject element
                                            && element.GetValueCI(CardConstants.KeyType) is JToken type
                                            && type.Type == JTokenType.String
                                            && type.ToString().EqualsCI(CardConstants.ActionSubmit)
                                        ? element : null)
                                    .WhereNotNull(), TreeNodeType.SubmitActionList).ConfigureAwait(false);
                        }, true).ConfigureAwait(false);
                })
            },
            {
                TreeNodeType.AnimationCard, new RichCardTreeNode<AnimationCard>(card => card.Buttons)
            },
            {
                TreeNodeType.AudioCard, new RichCardTreeNode<AudioCard>(card => card.Buttons)
            },
            {
                TreeNodeType.HeroCard, new RichCardTreeNode<HeroCard>(card => card.Buttons)
            },
            {
                TreeNodeType.OAuthCard, new RichCardTreeNode<OAuthCard>(card => card.Buttons)
            },
            {
                TreeNodeType.ReceiptCard, new RichCardTreeNode<ReceiptCard>(card => card.Buttons)
            },
            {
                TreeNodeType.SigninCard, new RichCardTreeNode<SigninCard>(card => card.Buttons)
            },
            {
                TreeNodeType.ThumbnailCard, new RichCardTreeNode<ThumbnailCard>(card => card.Buttons)
            },
            {
                TreeNodeType.VideoCard, new RichCardTreeNode<VideoCard>(card => card.Buttons)
            },
            {
                TreeNodeType.SubmitActionList, new ListTreeNode<object>(TreeNodeType.SubmitAction, PayloadIdTypes.Card)
            },
            {
                TreeNodeType.CardActionList, new ListTreeNode<CardAction>(TreeNodeType.CardAction, PayloadIdTypes.Card)
            },
            {
                TreeNodeType.SubmitAction, new TreeNode<object, JObject>(async (action, nextAsync) =>
                {
                    // If the entry point was the Adaptive Card or higher
                    // then the action will already be a JObject
                    return await action.ToJObjectAndBackAsync(
                        async actionJObject =>
                        {
                            if (actionJObject.GetValueCI(CardConstants.KeyData) is JObject data)
                            {
                                await nextAsync(data, TreeNodeType.Payload).ConfigureAwait(false);
                            }
                        }, true).ConfigureAwait(false);
                })
            },
            {
                TreeNodeType.CardAction, new TreeNode<CardAction, JObject>(async (action, nextAsync) =>
                {
                    if (action.Type == ActionTypes.MessageBack || action.Type == ActionTypes.PostBack)
                    {
                        async Task<T> CallNextAsync<T>(T input)
                            where T : class
                        {
                            return await input.ToJObjectAndBackAsync<T>(
                                async jObject => await nextAsync(jObject, TreeNodeType.Payload).ConfigureAwait(false),
                                true,
                                true).ConfigureAwait(false);
                        }

                        var valueResult = await CallNextAsync(action.Value).ConfigureAwait(false);

                        if (valueResult != null)
                        {
                            action.Value = valueResult;
                        }
                        else
                        {
                            var textResult = await CallNextAsync(action.Text).ConfigureAwait(false);

                            if (textResult != null)
                            {
                                action.Text = textResult;
                            }
                        }
                    }

                    return action;
                })
            },
            {
                TreeNodeType.Payload, new TreeNode<object, PayloadItem>(async (payload, nextAsync) =>
                {
                    return await payload.ToJObjectAndBackAsync(async payloadJObject =>
                    {
                        foreach (var type in PayloadIdTypes.Collection)
                        {
                            var id = payloadJObject.GetIdFromPayload(type);

                            if (id != null)
                            {
                                await nextAsync(new PayloadItem(type, id), TreeNodeType.Id);
                            }
                        }
                    }).ConfigureAwait(false);
                })
            },
            {
                TreeNodeType.Id, new TreeNode<PayloadItem, object>((id, _) => Task.FromResult(id))
            },
        };

        /// <summary>
        /// Enters and exits the tree at the specified nodes.
        /// </summary>
        /// <typeparam name="TEntry">The .NET type of the entry node.</typeparam>
        /// <typeparam name="TExit">The .NET type of the exit node.</typeparam>
        /// <param name="entryValue">The entry value.</param>
        /// <param name="action">A delegate to perform on each exit value.
        /// Note that exit values are not guaranteed to be non-null.</param>
        /// <param name="entryType">The explicit position of the entry node in the tree.
        /// If this is null then the position is inferred from the TEntry type parameter.
        /// Note that this parameter is required if the type is <see cref="object"/>
        /// or if the position otherwise cannot be unambiguously inferred from the type.</param>
        /// <param name="exitType">The explicit position of the exit node in the tree.
        /// If this is null then the position is inferred from the TExit type parameter.
        /// Note that this parameter is required if the type is <see cref="object"/>
        /// or if the position otherwise cannot be unambiguously inferred from the type.</param>
        /// <returns>The possibly-modified entry value. This is needed if a new object was created
        /// to modify the value, such as when an Adaptive Card is converted to a JObject.</returns>
        internal static TEntry Recurse<TEntry, TExit>(
            TEntry entryValue,
            Action<TExit> action,
            TreeNodeType? entryType = null,
            TreeNodeType? exitType = null)
            where TEntry : class
            where TExit : class
        {
            ITreeNode entryNode = null;
            ITreeNode exitNode = null;

            try
            {
                entryNode = GetNode<TEntry>(entryType);
            }
            catch (Exception ex)
            {
                throw GetNodeArgumentException<TEntry>(ex);
            }

            try
            {
                exitNode = GetNode<TExit>(exitType);
            }
            catch (Exception ex)
            {
                throw GetNodeArgumentException<TExit>(ex, "exit");
            }

            Task<object> Next(object child, TreeNodeType childType)
            {
                var childNode = _tree[childType];

                if (childNode == exitNode)
                {
                    action(GetExitValue<TExit>(child));

                    return Task.FromResult(child);
                }
                else
                {
                    return childNode.CallChildAsync(child, Next);
                }
            }

            return entryNode.CallChildAsync(entryValue, Next).Result as TEntry;
        }

        /// <summary>
        /// Enters and exits the tree at the specified nodes.
        /// </summary>
        /// <typeparam name="TEntry">The .NET type of the entry node.</typeparam>
        /// <typeparam name="TExit">The .NET type of the exit node.</typeparam>
        /// <param name="entryValue">The entry value.</param>
        /// <param name="funcAsync">A delegate to perform on each exit value.
        /// Note that exit values are not guaranteed to be non-null.</param>
        /// <param name="entryType">The explicit position of the entry node in the tree.
        /// If this is null then the position is inferred from the TEntry type parameter.
        /// Note that this parameter is required if the type is <see cref="object"/>
        /// or if the position otherwise cannot be unambiguously inferred from the type.</param>
        /// <param name="exitType">The explicit position of the exit node in the tree.
        /// If this is null then the position is inferred from the TExit type parameter.
        /// Note that this parameter is required if the type is <see cref="object"/>
        /// or if the position otherwise cannot be unambiguously inferred from the type.</param>
        /// <returns>The possibly-modified entry value. This is needed if a new object was created
        /// to modify the value, such as when an Adaptive Card is converted to a JObject.</returns>
        internal static async Task<TEntry> RecurseAsync<TEntry, TExit>(
            TEntry entryValue,
            Func<TExit, Task> funcAsync,
            TreeNodeType? entryType = null,
            TreeNodeType? exitType = null)
            where TEntry : class
            where TExit : class
        {
            ITreeNode entryNode = null;
            ITreeNode exitNode = null;

            try
            {
                entryNode = GetNode<TEntry>(entryType);
            }
            catch (Exception ex)
            {
                throw GetNodeArgumentException<TEntry>(ex);
            }

            try
            {
                exitNode = GetNode<TExit>(exitType);
            }
            catch (Exception ex)
            {
                throw GetNodeArgumentException<TExit>(ex, "exit");
            }

            async Task<object> NextAsync(object child, TreeNodeType childType)
            {
                var childNode = _tree[childType];

                if (childNode == exitNode)
                {
                    await funcAsync(GetExitValue<TExit>(child)).ConfigureAwait(false);

                    return child;
                }
                else
                {
                    return await childNode.CallChildAsync(child, NextAsync).ConfigureAwait(false);
                }
            }

            return await entryNode.CallChildAsync(entryValue, NextAsync).ConfigureAwait(false) as TEntry;
        }

        internal static TEntry ApplyIds<TEntry>(TEntry entryValue, PayloadIdOptions options = null, TreeNodeType? entryType = null)
            where TEntry : class
        {
            ITreeNode entryNode = null;

            try
            {
                entryNode = GetNode<TEntry>(entryType);
            }
            catch (Exception ex)
            {
                throw GetNodeArgumentException<TEntry>(ex);
            }

            void ProcessOptions(ITreeNode node)
            {
                if (node.IdType is string idType)
                {
                    options = (options ?? new PayloadIdOptions()).ReplaceNullWithGeneratedId(idType);
                    /*
                    foreach (var item in options.GetIdTypes()
                        .Where(it => PayloadIdTypes.GetIndex(it) < PayloadIdTypes.GetIndex(idType)))
                    {
                        options.Set(item);
                    }
                    */
                }
            }

            Task<object> Next(object child, TreeNodeType childType)
            {
                if (childType == TreeNodeType.Payload)
                {
                    // This local function is not async
                    // so just return the task without awaiting it
                    return child.ToJObjectAndBackAsync(
                        payload =>
                        {
                            payload.ApplyIdsToPayload(options);

                            return Task.CompletedTask;
                        }, true);
                }
                else
                {
                    if (childType == TreeNodeType.SubmitAction)
                    {
                        // We need to create a "data" object in the submit action
                        // if there isn't one already.
                        child = child.ToJObjectAndBackAsync(submitAction =>
                        {
                            if (submitAction.GetValueCI(CardConstants.KeyData).IsNullish())
                            {
                                submitAction.SetValueCI(CardConstants.KeyData, new JObject());
                            }

                            return Task.CompletedTask;
                        }).Result;
                    }

                    var childNode = _tree[childType];
                    var capturedOptions = options;

                    ProcessOptions(childNode);

                    // CallChild will be executed immediately even though its not awaited
                    var task = childNode.CallChildAsync(child, Next);

                    options = capturedOptions;

                    return task;
                }
            }

            ProcessOptions(entryNode);

            return entryNode.CallChildAsync(entryValue, Next).Result as TEntry;
        }

        internal static ISet<PayloadItem> GetIds<TEntry>(TEntry entryValue, TreeNodeType? entryType = null)
            where TEntry : class
        {
            var ids = new HashSet<PayloadItem>();

            Recurse(
                entryValue,
                (PayloadItem payloadId) =>
                {
                    ids.Add(payloadId);
                }, entryType);

            return ids;
        }

        private static TExit GetExitValue<TExit>(object child)
            where TExit : class => child is JToken jToken && !typeof(JToken).IsAssignableFrom(typeof(TExit)) ? jToken.ToObject<TExit>() : child as TExit;

        private static ITreeNode GetNode<T>(TreeNodeType? nodeType)
        {
            var t = typeof(T);

            if (nodeType is null)
            {
                if (t == typeof(object))
                {
                    throw new Exception("A node cannot be automatically determined from a System.Object type argument." + SpecifyManually);
                }

                var matchingNodes = new List<ITreeNode>();

                foreach (var possibleNode in _tree.Values)
                {
                    var possibleNodeTValue = possibleNode.GetTValue();

                    if (possibleNodeTValue.IsAssignableFrom(t) && possibleNodeTValue != typeof(object) && possibleNodeTValue != typeof(IEnumerable<object>))
                    {
                        matchingNodes.Add(possibleNode);
                    }
                }

                var count = matchingNodes.Count();

                if (count < 1)
                {
                    throw new Exception($"No node exists that's assignable from the type argument: {t}. Try using a different type.");
                }

                if (count > 1)
                {
                    throw new Exception($"Multiple nodes exist that are assignable from the type argument: {t}." + SpecifyManually);
                }

                return matchingNodes.First();
            }

            var exactNode = _tree[nodeType.Value];

            return exactNode.GetTValue().IsAssignableFrom(t)
                ? exactNode
                : throw new Exception($"The node type {nodeType} is not assignable from the type argument: {t}."
                    + " Make sure you're providing the correct node type.");
        }

        private static ArgumentException GetNodeArgumentException<TEntry>(Exception inner, string entryOrExit = "entry")
        {
            return new ArgumentException(
                $"The {entryOrExit} node could not be determined from the type argument: {typeof(TEntry)}.",
                $"{entryOrExit}Type",
                inner);
        }
    }
}
