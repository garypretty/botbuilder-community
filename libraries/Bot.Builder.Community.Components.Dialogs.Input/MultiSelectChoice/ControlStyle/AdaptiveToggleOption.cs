using System;
using System.Collections.Generic;
using System.Linq;
using AdaptiveCards;
using Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Setting;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;
using Newtonsoft.Json.Linq;

namespace Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.ControlStyle
{
    public class AdaptiveToggleOption : IAdaptiveControl
    {
        List<AdaptiveColumnSet> _adaptiveColumnSets;
        Orientation _orientationStyle;
        public List<AdaptiveColumnSet> PrepareControl(Orientation orientation, ChoiceSet choices)
        {
            _orientationStyle = orientation;

            switch (orientation)
            {
                case Orientation.Horizontal:
                    _adaptiveColumnSets = CreateHorizontalAdaptiveColumnSet(choices);
                    break;
                case Orientation.Vertical:
                    _adaptiveColumnSets = CreateVerticalAdaptiveColumnSet(choices);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }

            return _adaptiveColumnSets;
        }

        private static List<AdaptiveColumnSet> CreateHorizontalAdaptiveColumnSet(ChoiceSet choices)
        {
            var idx = 0;

            var adaptiveColumns = new List<AdaptiveColumnSet>();

            foreach (var choice in choices)
            {
                var columnSet = CreateAdaptiveColumnSet();
                columnSet.Columns.Add(CreateToggleInput(choice.Value, idx));
                idx++;
                adaptiveColumns.Add(columnSet);
            }

            return adaptiveColumns;
        }

        private static List<AdaptiveColumnSet> CreateVerticalAdaptiveColumnSet(ChoiceSet choices)
        {
            var columnSet = CreateAdaptiveColumnSet();

            var idx = 0;

            foreach (var choice in choices)
            {
                columnSet.Columns.Add(CreateToggleInput(choice.Value, idx));
                idx++;
            }

            return new List<AdaptiveColumnSet>() { columnSet };
        }

        private static AdaptiveColumnSet CreateAdaptiveColumnSet()
        {
            var columnSet = new AdaptiveColumnSet
            {
                Style = AdaptiveContainerStyle.Default,
                VerticalContentAlignment = AdaptiveVerticalContentAlignment.Center,
                Spacing = AdaptiveSpacing.Medium
            };

            return columnSet;
        }

        private static AdaptiveColumn CreateToggleInput(string value, int idx)
        {
            var toggleInput = new AdaptiveColumn { Type = "Column" };
            toggleInput.Items.Add(new AdaptiveToggleInput()
            {
                Id = idx.ToString(),
                Title = value,
                Value = "false",
                Height = AdaptiveHeight.Auto,
                
            });
            toggleInput.Width = "auto";
            toggleInput.Spacing = AdaptiveSpacing.Medium;

            return toggleInput;
        }


        public List<ResultSet> ResultSet(object validateText)
        {
            if (_adaptiveColumnSets == null)
                return null;

            List<ResultSet> result = null;

            if (_adaptiveColumnSets == null && !(_adaptiveColumnSets?.Count > 0))
            {
                return null;
            }

            var selectedSet = JObject.FromObject(validateText).ToObject<Dictionary<string, bool>>();

            var selectedItems = selectedSet?.Where(selectItem => selectItem.Value).ToList();

            if (selectedItems == null)
            {
                return null;
            }

            result = JObjectResult(selectedItems);

            return result;
        }

        private List<ResultSet> JObjectResult(IEnumerable<KeyValuePair<string, bool>> selectedItems)
        {
            List<ResultSet> result;
            switch (_orientationStyle)
            {
                case Orientation.Horizontal:
                    result = HorizontalResultSet(selectedItems);
                    break;
                case Orientation.Vertical:
                    result = VerticalResultSet(_adaptiveColumnSets[0],selectedItems);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        private List<ResultSet> HorizontalResultSet(IEnumerable<KeyValuePair<string, bool>> selectedDictionary)
        {
            var result = new List<ResultSet>();

            foreach (var selectedItem in selectedDictionary)
            {
                foreach (var columnInfo in _adaptiveColumnSets)
                {
                    var resultSet = PrepareResult(columnInfo.Columns[0], selectedItem.Key);

                    if (resultSet != null)
                    {
                        result.Add(resultSet);
                        break;
                    }
                }
            }

            return result;
        }

        private static List<ResultSet> VerticalResultSet(AdaptiveColumnSet adaptiveColumnSet,IEnumerable<KeyValuePair<string, bool>> selectedDictionary)
        {
            var result = new List<ResultSet>();

            foreach (var selectItem in selectedDictionary)
            {
                foreach (var columnInfo in adaptiveColumnSet.Columns)
                {
                    var resultSet = PrepareResult(columnInfo, selectItem.Key);
                    if (resultSet != null)
                    {
                        result.Add(resultSet);
                        break;
                    }
                }
            }

            return result;
        }

        private static ResultSet PrepareResult(AdaptiveColumn columnInfo, string selectItem)
        {
            foreach (var item in columnInfo.Items.Where(id => id.Id == selectItem))
            {
                if (item is AdaptiveToggleInput selectInput)
                {
                    return new ResultSet
                    {
                        Index = Convert.ToInt32(selectItem),
                        Value = selectInput.Title
                    };
                }
            }

            return null;
        }
    }
}
