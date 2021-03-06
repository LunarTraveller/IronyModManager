﻿// ***********************************************************************
// Assembly         : IronyModManager
// Author           : Mario
// Created          : 03-03-2020
//
// Last Modified By : Mario
// Last Modified On : 01-22-2021
// ***********************************************************************
// <copyright file="CollectionModsControlView.xaml.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using IronyModManager.Common.Views;
using IronyModManager.Controls;
using IronyModManager.Models.Common;
using IronyModManager.Shared;
using IronyModManager.ViewModels.Controls;
using ReactiveUI;

namespace IronyModManager.Views.Controls
{
    /// <summary>
    /// Class CollectionModsControlView.
    /// Implements the <see cref="IronyModManager.Common.Views.BaseControl{IronyModManager.ViewModels.Controls.CollectionModsControlViewModel}" />
    /// </summary>
    /// <seealso cref="IronyModManager.Common.Views.BaseControl{IronyModManager.ViewModels.Controls.CollectionModsControlViewModel}" />
    [ExcludeFromCoverage("This should be tested via functional testing.")]
    public class CollectionModsControlView : BaseControl<CollectionModsControlViewModel>
    {
        #region Fields

        /// <summary>
        /// The order name
        /// </summary>
        private const string OrderName = "order";

        /// <summary>
        /// The mod list
        /// </summary>
        private DragDropListBox modList;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionModsControlView" /> class.
        /// </summary>
        public CollectionModsControlView()
        {
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// focus order text box as an asynchronous operation.
        /// </summary>
        /// <param name="mod">The mod.</param>
        protected virtual async Task FocusOrderTextBoxAsync(IMod mod)
        {
            await Task.Delay(100);
            var listboxItems = modList.GetLogicalChildren().Cast<ListBoxItem>();
            if (mod != null)
            {
                foreach (var item in listboxItems)
                {
                    var grid = item.GetLogicalChildren().OfType<Grid>().FirstOrDefault();
                    if (grid != null)
                    {
                        var contentMod = item.Content as IMod;
                        if (mod == contentMod)
                        {
                            grid.Focus();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the item dragged.
        /// </summary>
        protected virtual void HandleItemDragged()
        {
            modList.ItemDragged += (source, destination) =>
            {
                var sourceMod = source as IMod;
                var destinationMod = destination as IMod;
                ViewModel.InstantReorderSelectedItems(sourceMod, destinationMod.Order);
            };
        }

        /// <summary>
        /// Called when [activated].
        /// </summary>
        /// <param name="disposables">The disposables.</param>
        protected override void OnActivated(CompositeDisposable disposables)
        {
            modList = this.FindControl<DragDropListBox>("modList");
            if (modList != null)
            {
                SetContextMenus();
                SetOrderParameters();
                HandleItemDragged();
            }
            base.OnActivated(disposables);
        }

        /// <summary>
        /// Sets the pointer events.
        /// </summary>
        protected virtual void SetContextMenus()
        {
            modList.ContextMenuOpening += (sender, args) =>
            {
                List<MenuItem> menuItems = null;
                var hoveredItem = modList.GetLogicalChildren().Cast<ListBoxItem>().FirstOrDefault(p => p.IsPointerOver);
                if (hoveredItem != null)
                {
                    ViewModel.HoveredMod = hoveredItem.Content as IMod;
                    menuItems = GetMenuItems();
                }
                if (modList.ItemCount == 0)
                {
                    menuItems = GetStaticMenuItems();
                }
                modList.SetContextMenu(menuItems);
            };
        }

        /// <summary>
        /// Sets the order parameters.
        /// </summary>
        protected virtual void SetOrderParameters()
        {
            void setNumericProperties(bool setMargin = false)
            {
                var listboxItems = modList.GetLogicalChildren().Cast<ListBoxItem>();
                foreach (var item in listboxItems)
                {
                    var grid = item.GetLogicalChildren().OfType<Grid>().FirstOrDefault();
                    if (grid != null)
                    {
                        var orderCtrl = grid.GetLogicalChildren().OfType<MinMaxNumericUpDown>().FirstOrDefault(p => p.Name == OrderName);
                        if (orderCtrl != null)
                        {
                            orderCtrl.Minimum = 1;
                            orderCtrl.Maximum = ViewModel.MaxOrder;
                            if (setMargin)
                            {
                                var left = 0;
                                var right = 0;
                                if (orderCtrl.Bounds.Width > 100)
                                {
                                    right = 10;
                                }
                                else
                                {
                                    left = 6;
                                }
                                orderCtrl.Margin = new Avalonia.Thickness(left, 0, right, 0);
                            }
                        }
                    }
                }
            }

            ViewModel.ModReordered += (mod, instant) =>
            {
                setNumericProperties(instant);
                modList.Focus();
                FocusOrderTextBoxAsync(mod).ConfigureAwait(true);
            };

            this.WhenAnyValue(v => v.ViewModel.MaxOrder).Subscribe(max =>
            {
                if (Dispatcher.UIThread.CheckAccess())
                {
                    setNumericProperties();
                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(() => setNumericProperties());
                }
            }).DisposeWith(Disposables);

            modList.LayoutUpdated += (sender, args) =>
            {
                setNumericProperties(true);
            };
        }

        /// <summary>
        /// Gets the menu items.
        /// </summary>
        /// <returns>List&lt;MenuItem&gt;.</returns>
        private List<MenuItem> GetMenuItems()
        {
            List<MenuItem> menuItems = null;
            if (!string.IsNullOrEmpty(ViewModel.GetHoveredModUrl()) || !string.IsNullOrEmpty(ViewModel.GetHoveredModSteamUrl()) || !string.IsNullOrWhiteSpace(ViewModel.HoveredMod?.FullPath))
            {
                menuItems = new List<MenuItem>
                {
                    new MenuItem()
                    {
                        Header = ViewModel.CollectionJumpOnPositionChangeLabel,
                        Command = ViewModel.CollectionJumpOnPositionChangeCommand
                    },
                    new MenuItem()
                    {
                        Header = "-"
                    },
                    new MenuItem()
                    {
                        Header = ViewModel.ExportCollectionToClipboard,
                        Command = ViewModel.ExportCollectionToClipboardCommand
                    },
                    new MenuItem()
                    {
                        Header = ViewModel.ImportCollectionFromClipboard,
                        Command = ViewModel.ImportCollectionFromClipboardCommand
                    },
                    new MenuItem()
                    {
                        Header = "-"
                    }
                };
                var counterOffset = 5;
                if (ViewModel.CanExportModHashReport)
                {
                    menuItems.Add(new MenuItem()
                    {
                        Header = ViewModel.ExportReport,
                        Command = ViewModel.ExportReportCommand
                    });
                    menuItems.Add(new MenuItem()
                    {
                        Header = ViewModel.ImportReport,
                        Command = ViewModel.ImportReportCommand
                    });
                    menuItems.Add(new MenuItem()
                    {
                        Header = "-"
                    });
                    counterOffset += 3;
                }
                if (!string.IsNullOrEmpty(ViewModel.GetHoveredModUrl()))
                {
                    menuItems.Add(new MenuItem()
                    {
                        Header = ViewModel.OpenUrl,
                        Command = ViewModel.OpenUrlCommand
                    });
                    menuItems.Add(new MenuItem()
                    {
                        Header = ViewModel.CopyUrl,
                        Command = ViewModel.CopyUrlCommand
                    });
                }
                if (!string.IsNullOrEmpty(ViewModel.GetHoveredModSteamUrl()))
                {
                    var menuItem = new MenuItem()
                    {
                        Header = ViewModel.OpenInSteam,
                        Command = ViewModel.OpenInSteamCommand
                    };
                    if (menuItems.Count == counterOffset)
                    {
                        menuItems.Add(menuItem);
                    }
                    else
                    {
                        menuItems.Insert(counterOffset + 1, menuItem);
                    }
                }
                if (!string.IsNullOrWhiteSpace(ViewModel.HoveredMod?.FullPath))
                {
                    var menuItem = new MenuItem()
                    {
                        Header = ViewModel.OpenInAssociatedApp,
                        Command = ViewModel.OpenInAssociatedAppCommand
                    };
                    if (menuItems.Count == counterOffset)
                    {
                        menuItems.Add(menuItem);
                    }
                    else
                    {
                        menuItems.Insert(counterOffset, menuItem);
                    }
                }
            }
            return menuItems;
        }

        /// <summary>
        /// Gets the static menu items.
        /// </summary>
        /// <returns>List&lt;MenuItem&gt;.</returns>
        private List<MenuItem> GetStaticMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem()
                {
                    Header = ViewModel.CollectionJumpOnPositionChangeLabel,
                    Command = ViewModel.CollectionJumpOnPositionChangeCommand
                },
                new MenuItem()
                {
                    Header = "-"
                },
                new MenuItem()
                {
                    Header = ViewModel.ImportCollectionFromClipboard,
                    Command = ViewModel.ImportCollectionFromClipboardCommand
                }
            };
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #endregion Methods
    }
}
