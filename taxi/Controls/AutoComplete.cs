using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;

namespace taxi
{
	public class AutoComplete : StackLayout
	{
		private Entry entry;
		private ListView listview;
		private Button button;

		private List<string> listSource = new List<string> { 
		  "Ivan",
          "Semen",
          "Stepan" 
		};


		public AutoComplete()
		{
			init();
		}

		#region Bindable Properties

		public static BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(AutoComplete), defaultBindingMode: BindingMode.TwoWay, propertyChanged: onTextChanged);
		public static BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource",typeof(List<string>),typeof(AutoComplete), propertyChanged: onItemSourceChanged);
		public static BindableProperty DoneCommandProperty = BindableProperty.Create("DoneCommand",typeof(ICommand),typeof(AutoComplete));
		public static BindableProperty TextChangedCommandProperty = BindableProperty.Create("TextChangedCommand",typeof(ICommand),typeof(AutoComplete));
		#endregion

		#region Properties

		public string Text
		{
			get {
				return (string) GetValue(TextProperty);
			}
			set {				SetValue(TextProperty, value);
			}
		}


		public List<string> ItemsSource
		{
			get
			{
				return (List<string>) GetValue(ItemsSourceProperty);	
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public ICommand DoneCommand
		{
			get
			{
				return (ICommand) GetValue(DoneCommandProperty);
			}
			set
			{
				SetValue(DoneCommandProperty, value);
			}
		}
		public ICommand TextChangedCommand
		{
			get
			{
				return (ICommand) GetValue(TextChangedCommandProperty);
			}
			set
			{
				SetValue(TextChangedCommandProperty, value);
			}
		}

		#endregion


		#region Properties Changed

		static void onItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var autoCompleteControl = bindable as AutoComplete;

			if (autoCompleteControl != null)
			{
				if(newValue is List<string>){

					autoCompleteControl.listview.ItemsSource = (List<string>)newValue;
				}else{
					autoCompleteControl.listview.ItemsSource = new List<string>();
				}
			}
		}

		static void onTextChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var autoCompleteControl = bindable as AutoComplete;

			if (autoCompleteControl != null)
			{
				var value = newValue as string;
				if (value != null && autoCompleteControl.entry.Text != value)
				{
					autoCompleteControl.entry.Text = value;
				}


			}
		}
        #endregion



		void init()
		{
			button = new Button{
				Text = "OK"
			};
			button.Clicked += onDoneButtonClicked;
				
				      
			Orientation = StackOrientation.Vertical;

			entry = new Entry{
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			entry.TextChanged += onEntryTextChanged;


		

			listview = new ListView
			{
				ItemsSource = listSource
			};

			listview.IsVisible = true;
			listview.SeparatorVisibility = SeparatorVisibility.None;
			listview.HasUnevenRows = true;
			listview.RowHeight = 20;
			listview.ItemTapped+= onListItemTapped;


			var horStack = new StackLayout{
				Orientation= StackOrientation.Horizontal
			};

			horStack.Children.Add(entry);
			horStack.Children.Add(button);

			Children.Add(horStack);
			Children.Add(listview);


		
				
		}

		void onDoneButtonClicked(object sender, EventArgs e)
		{
			if(DoneCommand != null && DoneCommand.CanExecute(null)){
				DoneCommand.Execute(null);
			}
		}

		void onListItemTapped(object sender, ItemTappedEventArgs e)
		{
			var selectedItem = listview.SelectedItem as string;
			if(selectedItem != null){
				Text = selectedItem;
				listview.SelectedItem = null;
			}
		}

		void onEntryTextChanged(object sender, TextChangedEventArgs e)
		{
			Text = entry.Text;

			if(entry.Text.Length > 2 && TextChangedCommand != null && TextChangedCommand.CanExecute(null)){
				TextChangedCommand.Execute(null);
			}else{
				if(ItemsSource != null && ItemsSource.Count > 0)
					listview.ItemsSource = new List<string>();
			}

			//listview.ItemsSource = new List<string> {

			//		"Semen",
			//		"Stepan"
			//	};

		}

		protected override void OnRemoved(View view)
		{
			base.OnRemoved(view);
			entry.TextChanged -= onEntryTextChanged;
			button.Clicked -= onDoneButtonClicked;
			listview.ItemTapped -= onListItemTapped;
		}
}
}
