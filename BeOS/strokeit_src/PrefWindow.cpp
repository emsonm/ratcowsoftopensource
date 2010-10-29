#include "PrefWindow.h"


PrefWindow::PrefWindow() : 
  BWindow( 
    BRect(100, 100, 500, 300), 
    "StrokeIt Settings",
    B_FLOATING_WINDOW_LOOK,
    B_NORMAL_WINDOW_FEEL,
    0 ) 
{ 
  main_save_file.SetTo("");
  trainer_save_file.SetTo("");

  /////////////////////////////////////////////////////////////////
  // Read the settings
  SettingsFile s("strokeit", "strokeit");
  s.Load();
  if (!s.FindString("strokefile", &main_save_file)==B_OK)
  {
    main_save_file.SetTo("(none)");   
  }

  s.FindBool("help", &help_value);
  s.FindInt32("scale", &scale_value);
  /////////////////////////////////////////////////////////////////


  /////////////////////////////////////////////////////////////////
  // Set up the tabview   
  BRect r = Bounds();
   
  _view = new BTabView(r, "pref_tabview");

  r = _view->Bounds();
  r.InsetBy(5,5);
  r.bottom -= _view->TabHeight();

  tab1 = new BTab();
  PrefView* tab1_view = new PrefView(r); 
  _view->AddTab(tab1_view, tab1);
  tab1->SetLabel("Main");

  tab2 = new BTab();
  PrefView* tab2_view = new PrefView(r);
  _view->AddTab(tab2_view, tab2);
  tab2->SetLabel("Trainer");

  AddChild(_view);

  /////////////////////////////////////////////////////////////////

  //_btn = new BButton(
  //  BRect(3, 25, 45, 53),
  //  "TestBtn",
  //  "Test",
  //  new BMessage('tstB'),
  //  0 
  //);
   
  //tab1_view->AddChild(_btn); 

  /////////////////////////////////////////////////////////////////
  // Set up the trainer view
  stroke_view = new PrefStrokeView(5, 5);
  tab2_view->AddChild( stroke_view );

  traineroutput.SetTo("");

  trainer_output = new BTextControl(BRect(160, 3, 350, 30), "trainerInput", "stroke", "", new BMessage('trnO') );
  trainer_output->SetEnabled(false);
  trainer_output->SetDivider( 60 );
  tab2_view->AddChild( trainer_output );

  trainer_input = new BTextControl(BRect(160, 35, 350, 60), "trainerInput", "translation", "", new BMessage('trnI') );
  //trainer_input->SetEnabled(false);
  trainer_input->SetDivider( 60 );
  tab2_view->AddChild( trainer_input );

  trainer_button_save = new BButton(BRect(160, 65, 280, 95), "trainerSaveBtn", "Save to active file", new BMessage('svTf'));
  //trainer_button->SetEnabled(false);
  tab2_view->AddChild( trainer_button_save );

  trainer_button_saveto = new BButton(BRect(160, 100, 280, 130), "trainerSaveToBtn", "Save to selected file", new BMessage('sTsf'));
  //trainer_button->SetEnabled(false);
  tab2_view->AddChild( trainer_button_saveto );

  /////////////////////////////////////////////////////////////////

  /////////////////////////////////////////////////////////////////
  // Set up the Main view
  help_setting = new BCheckBox(BRect(3, 35, 77, 55), "helpSeting", "Bubble help", new BMessage('chHp')); 
  tab1_view->AddChild(help_setting); 

  help_setting->SetValue(help_value); 

  scale_setting = new BSlider(BRect(3, 105, 157, 185), "scaleSlider", "Scale:", new BMessage('stSc'), 1, 4 ); //, B_TRIANGLE_THUMB);
  scale_setting->SetHashMarks(B_HASH_MARKS_TOP);
  tab1_view->AddChild(scale_setting);

  cout << "scale value is " << scale_value << endl;
  scale_setting->SetValue( scaleToSlider(scale_value) );
  scale_setting->SetModificationMessage( new BMessage('ScMv') ); 
  
  cout << "we picked " << scale_setting->Value() << " as a good match" << endl;
  scale_setting->SetLimitLabels("smallest", "largest");
  setSliderLabel();
  /////////////////////////////////////////////////////////////////
    
  /////////////////////////////////////////////////////////////////
  // Set up the pop-up menus...
  BPopUpMenu*  file_list_menu = new BPopUpMenu(""); //Main : for file selection
  BPopUpMenu*  trainer_file_list_menu = new BPopUpMenu(""); //Trainer: for "save stroke into"...
   
  BPath settingspath;

  //set up the file list
  if ((find_directory(B_USER_SETTINGS_DIRECTORY, &settingspath))==B_OK)
  {
    //found path to settings...
    cout << "The path to the settings is... " << settingspath.Path() << endl;

    if (settingspath.Append("strokeit/strokes") == B_OK)
      settings_dir = new BDirectory( settingspath.Path() ); 
    else return;

    BEntry entry;
    BPath file;
    while (settings_dir->GetNextEntry(&entry)==B_OK)
    {
      file.SetTo(&entry); 
      cout << file.Path() << endl;

      BMessage* tmp = new BMessage('mnuF');
      tmp->AddString( "path", file.Path() );
      tmp->AddString( "filename", file.Leaf() ); 
      BMenuItem* mnu = new BMenuItem(file.Leaf(), tmp);
      file_list_menu->AddItem( mnu );

      BMessage* tmp2 = new BMessage('mnF2');
      tmp2->AddString( "path", file.Path() );
      tmp2->AddString( "filename", file.Leaf() ); 
      BMenuItem* mnu2 = new BMenuItem(file.Leaf(), tmp2);
      trainer_file_list_menu->AddItem( mnu2 );

      if ( main_save_file.Compare( file.Leaf() )==0 )
      {
        cout << "bing!!" << endl;
        mnu->SetMarked(true);
        mnu2->SetMarked(true);
      }
      trainer_save_file.SetTo( main_save_file.String() );
    }           
  } 
  /////////////////////////////////////////////////////////////////


  /////////////////////////////////////////////////////////////////
  // Set up the 
  file_list = new BMenuField(BRect(3, 3, 300, 28), "mnufldFileLst", "Stroke File", file_list_menu); 
  file_list->SetDivider(60);
  tab1_view->AddChild(file_list); 

  trainer_file_list = new BMenuField(BRect(290, 100, 457, 128), "mnufldTrainerFLst", "", trainer_file_list_menu);  
  trainer_file_list->SetDivider(1);
  tab2_view->AddChild(trainer_file_list); 
  /////////////////////////////////////////////////////////////////
}




PrefWindow::~PrefWindow()
{
  delete settings_dir;
}



/////////////////////////////////////////////////////////////////

int32 PrefWindow::sliderToScale( int32 value )
{
  cout << " ( value is " << value << ") ";

  switch( value )
  {
    case 1:
      return 25;
    case 2:
      return 40;
    case 3:
      return 50;
    case 4:
      return 60;
    default:
      return 50;
  } 
}

///////////////////////////////////////////////

int32 PrefWindow::scaleToSlider( int32 value )
{
  cout << " ( value is " << value << ") ";

  switch(value)
  {
    case 25:
      return 1;
    case 40:
      return 2; 
    case 50:
      return 3;
    case 60:
      return 4;
    default:
      return 3;
  }
}

///////////////////////////////////////////////

void  PrefWindow::setSliderLabel()
{
  BString tmp;
  tmp << "Scale:  " ;

  switch( scaleToSlider(scale_value) )
  {
    case 1:
      tmp << "small";
      break;
    case 2:
      tmp << "medium";
      break; 
    case 3:
      tmp << "normal";
      break;
    case 4:
      tmp << "large";
      break; 
    default:
      tmp << "default"; 
      break;
  }

  scale_setting->SetLabel( tmp.String() );

}

/////////////////////////////////////////////////////////////////


void PrefWindow::MessageReceived( BMessage* message )
{
   BString tmp("");

   switch (message->what)
  {
    //case 'tstB':
    //  signal_app();
    //  break;

    case 'chHp':
      cout << "help value reset ";
      if (help_value) cout << "was true "; else cout << "was false "; 
      help_value = help_setting->Value();
      if (help_value) cout << "now true "; else cout << "now false "; 
      cout << endl;
      save_prefs(); 
      break;

    case 'mnuF':
      cout << "got a BMessageField selection message..." << endl;
      if (message->FindString("filename", &tmp)==B_OK)
      {
        main_save_file.SetTo( tmp.String() );
        cout << tmp.String() << endl;
        save_prefs();
      } 
      break;

    case 'mnF2':
      cout << "got a BMessageField selection message..." << endl;
      if (message->FindString("filename", &tmp)==B_OK)
      {
        trainer_save_file.SetTo( tmp.String() );
        cout << tmp.String() << endl;
      } 
      break;

    case 'svTf':
      cout << "got a file save message...(save current)" << endl;
      tmp.SetTo( trainer_input->Text() );
      save_stroke( tmp.String(), main_save_file.String() );
      break;

    case 'sTsf':
      cout << "got a file save message...(save into)" << endl;
      tmp.SetTo( trainer_input->Text() );
      save_stroke( tmp.String(), trainer_save_file.String() );
      break;

    case 'stSc':
      cout << "scale changed: was " << scale_value << " ";
      scale_value = sliderToScale( scale_setting->Value() );
      cout << "now " << scale_value << endl;
      save_prefs();
      setSliderLabel();
      break;

    case 'ScMv':
      cout << "track move " << scale_value << endl;
      break;
     
    default:
      BWindow::MessageReceived(message);
      break;
  }
}



bool PrefWindow::QuitRequested()
{
  save_prefs();
  signal_app();

  be_app->PostMessage(B_QUIT_REQUESTED);
	return(true);
}



void PrefWindow::update_trainer_output(const char* _stroke)
{
  traineroutput.SetTo(_stroke);  
  trainer_output->SetText( traineroutput.String() );
  trainer_input->SetText( "" );
}



void PrefWindow::save_stroke(const char* stroke, const char* filename)
{
  cout << "save stroke '" << traineroutput.String() << "' as '" << stroke << "'" << endl;

  BFile strokes;
  BEntry entry;
  BPath settingspath; 

  if (stroke[0]==0) return;

  if ((find_directory(B_USER_SETTINGS_DIRECTORY, &settingspath))==B_OK)
  {
    //found path to settings...
    cout << "The path to the settings is... " << settingspath.Path() << endl;

    settingspath.Append("strokeit/strokes/");
    settingspath.Append( filename ); //selected_file.String() );

    cout << "Using for strokefile : "<< settingspath.Path() << endl; 
    
    entry.SetTo( settingspath.Path() );

    if (B_OK != strokes.SetTo(&entry, B_READ_WRITE))
    {
      cout << "no strokes" << endl; 
      return;
    }
    else 
    {
      cout << "strokes loaded" << endl;
      
      off_t _size;
      strokes.GetSize(&_size);
      cout << "size is " << _size << endl;

      strokes.Seek(_size, 0); 
      uint32 pos = strokes.Position();
      cout << "pos is " << pos << endl;
       
      char ch;
      strokes.Seek(-1, SEEK_END); 
      strokes.Read(&ch, sizeof(char)); //go back one

      int32 _offset = 0; 
      if (ch==0x0a)
        _offset = -4; // "0=0\n" 
      else 
        _offset = -3; // "0=0"

      strokes.Seek(_offset, SEEK_END); //go back a few chars..

      char tmp[4]= {0, 0, 0, 0};
      strokes.Read( &tmp, sizeof(tmp) );
      tmp[3] = 0; //make sure we kill off the last char...

      BString tmp_in;
      tmp_in << tmp;

      cout << "found... " << tmp_in.String() << endl;
      if (tmp_in.Compare("0=0")==0)
        strokes.Seek(_offset, SEEK_END); //remove the eof char sequence...

      tmp_in.SetTo(""); 
      tmp_in << trainer_output->Text() << "=" << stroke << (char)0x0a;
      cout << "we have ... "<< tmp_in.String(); 
      strokes.Write( tmp_in.String(), tmp_in.CountChars() );

      tmp_in.SetTo(""); 
      tmp_in << "0=0" << (char)0x0a;
      cout << "we have ... "<< tmp_in.String(); 
      strokes.Write( tmp_in.String(), tmp_in.CountChars() );  
    }          
  }
  update_trainer_output("");
}



void PrefWindow::signal_app()
{
  //signal pref save to app if running...
  BMessenger *msgr = new BMessenger("application/x-vnd.StrokeIt.ME");
  if (msgr->IsValid())
    msgr->SendMessage( new BMessage('srlD') ); 
  delete msgr;
}



void PrefWindow::save_prefs()
{
  //update prefs
  SettingsFile s("strokeit", "strokeit");
  s.Load();
  s.ReplaceString( "strokefile", main_save_file.String() );
  s.ReplaceBool( "help", help_value );
  s.ReplaceInt32( "scale", scale_value );
  s.Save();
}
