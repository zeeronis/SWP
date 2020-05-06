using GongSolutions.Wpf.DragDrop;
using SWP.Classes.Repository;
using System;
using System.Collections.ObjectModel;
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
            var sourceItem = (UnitItem)dropInfo.Data;
            var targetItem = (UnitItem)dropInfo.TargetItem;
            var sourceCollection = dropInfo.DragInfo.SourceCollection as ObservableCollection<UnitItem>;
            var targetCollection = dropInfo.TargetCollection as ObservableCollection<UnitItem>;

            if (sourceItem != null && targetCollection != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                if(sourceCollection == targetCollection)
                {
                    dropInfo.Effects = DragDropEffects.Move;
                }
                else
                {
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
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

                    UnitsRepo.SetNeedSave();
                    CollectionViewSource.GetDefaultView(targetCollection).Refresh();
                    break;

                case DragDropEffects.Move:
                    int id = sourceItem.ID;
                    sourceItem.SetID(targetItem.ID);
                    targetItem.SetID(id);

                    UnitsRepo.SetNeedSave();
                    CollectionViewSource.GetDefaultView(targetCollection).Refresh();
                    break;

                default:
                    break;
            }
        }
    }
}
