using Sirenix.OdinInspector;
using System;

namespace SuperstarDJ.CustomEditors.CompositeAttributes
{
    [IncludeMyAttributes]
    [ShowInInspector]
    [ListDrawerSettings ( IsReadOnly = true, ShowItemCount = true, ShowIndexLabels = true, NumberOfItemsPerPage = 32, OnBeginListElementGUI = "OnBeginListElementGUI", OnEndListElementGUI = "OnAfterEnumisGUI" )]
    [EnumToggleButtons]
    [LabelWidth ( 50 )]
    class DisplayPatternEditAttribute : Attribute { }
}
