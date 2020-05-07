using GongSolutions.Wpf.DragDrop;
using SWP.Classes.Repository;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace SWP.Classes.Game
{
    [Serializable]
    public class Team: IDropTarget
    {
        public ContentType ContentType { get; set; }
        public ObservableCollection<UnitItem> UnitsList { get; set; } = new ObservableCollection<UnitItem>();

        public void DragOver(IDropInfo dropInfo)
        {
            bool isFilterCollection = false;
            Type SourceCollectionType = dropInfo.DragInfo.SourceCollection.GetType();

            if (SourceCollectionType != typeof(ObservableCollection<UnitItem>)
                || dropInfo.TargetCollection.GetType() != typeof(ObservableCollection<UnitItem>))
            {
                if (SourceCollectionType == typeof(ListCollectionView))
                {
                    isFilterCollection = true;
                }
                else
                {
                    return;
                }
            }

            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = isFilterCollection
                    ? DragDropEffects.Copy
                    : DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            var sourceItem = (UnitItem)dropInfo.Data;
            var targetItem = (UnitItem)dropInfo.TargetItem;
            var sourceCollection = dropInfo.DragInfo.SourceCollection as ObservableCollection<UnitItem>;
            var targetCollection = dropInfo.TargetCollection as ObservableCollection<UnitItem>;

            switch (dropInfo.Effects)
            {
                case DragDropEffects.Copy:
                    targetItem.SetID(sourceItem.ID);

                    Core.MarkTeamsAsChanged();
                    CollectionViewSource.GetDefaultView(targetCollection).Refresh();
                    break;

                case DragDropEffects.Move:
                    int id = sourceItem.ID;
                    sourceItem.SetID(targetItem.ID);
                    targetItem.SetID(id);

                    Core.MarkTeamsAsChanged();
                    CollectionViewSource.GetDefaultView(targetCollection).Refresh();
                    CollectionViewSource.GetDefaultView(sourceCollection).Refresh();
                    break;

                default:
                    break;
            }
        }
    }
}
