using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Windows.Threading;
using DemoMQTTSparkplugB.Core;
using DemoMQTTSparkplugB.Model;
using System.Text.RegularExpressions;

namespace DemoMQTTSparkplugB.ViewModel
{
    class MainViewModel : ObservableObject
    {
        // Convert datetime to timestamp
        private static double ConvertDateTimeToTimestamp(DateTime value)
        {
            TimeSpan epoch = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (double)epoch.TotalSeconds;
        }

        // Connect server client 1
        ConnectServer1 cntServer1 = new ConnectServer1();

        // Connect server client 2
        ConnectServer2 cntServer2 = new ConnectServer2();

        public string CurrentTopicSub { get; set; }

        private string _currentTopic;

        public string CurrentTopic
        {
            get { return _currentTopic; }
            set { _currentTopic = value; OnPropertyChanged(); }
        }

        public string CurrentPayload { get; set; }
    
        public ICommand LoadWindowCommand { set; get; }

        //Node*************************************
        // use for NDEATH , NBIRTH
        public bool RebootNode { get; set; }
        public bool RebootNodeUnchecked { get; set; }
        public double TimeRebootNode { get; set; }
        public double TimeRebootNodeUnchecked { get; set; }

        public bool RebirthNode { get; set; }
        public bool RebirthNodeUnchecked { get; set; }
        public double TimeRebirthNode { get; set; }
        public double TimeRebirthNodeUnchecked { get; set; }

        public int ScanNodeValue { get; set; }
        public double SupVoltValue { get; set; }


        public double TimeCntAndPublish { get; set; }
        public double TimeDisCnt { get; set; }
        public int seqBirth { get; set; }
        public bool ConnectDeadState { get; set; }

        public ICommand RebootNodeCommand { get; set; }
        public ICommand RebirthNodeCommand { get; set; }     
        public ICommand CntNodeCommand { get; set; }

        // use for NDATA , NCMD
        public ICommand PublishDataNodeCommand { get; set; }  
        public double TimeDataPublish { get; set; }
        public int seqNDATA { get; set; }
        public ICommand PublishCmdNodeCommand { get; set; }
        public double TimeCmdPublish { get; set; }



        // Device*************************************
        // use for DDEATH , DBIRTH         
        public bool ValueInputChecked { get; set; }
        public bool ValueInputUnchecked { get; set; }
        public double TimeInputChecked { get; set; }
        public double TimeInputUnchecked { get; set; }

        public double TimeCntAndPublishDev { get; set; }
        public double TimeDisCntDev { get; set; }
        public int seqDBirth { get; set; }
        public bool ConnectDeadStateDev { get; set; }
        public int seqDDeath { get; set; }

        public ICommand CntDevCommand { get; set; }
        public ICommand Input { get; set; }

        private string _valueOutput;

        public string ValueOutput
        {
            get
            { 
                return _valueOutput; 
            }
            set 
            { _valueOutput = value;
               OnPropertyChanged();
            }
        }

        // use for DDATA , DCMD
        public ICommand PublishDataDevCommand { get; set; }
        public double TimeDataDevPublish { get; set; }
        public int seqDDATA { get; set; }
        public ICommand PublishCmdDevCommand { get; set; }
        public double TimeCmdDevPublish { get; set; }


        // Client2 control
        public ICommand SubClick { get; set; }
        private string _topicSubcribe;

        public string TopicSubcribe
        {
            get 
            {
                return _topicSubcribe; 
            }
            set 
            { 
                _topicSubcribe = value;
                OnPropertyChanged();
            }
        }
        private string _currentMess;

        public string CurrentMess
        {
            get
            { 
                return _currentMess; 
            }
            set 
            { 
                _currentMess = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            ConnectDeadState = true;
            ScanNodeValue = 3000;
            SupVoltValue = 12;


            ConnectDeadStateDev = true;
            ValueOutput = "false";


            TopicSubcribe = "spBv1.0/Demo/NBIRTH/RaspberryPi";

            


            // create timer 
            LoadWindowCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; },  (p) =>
            {
                if (p != null)
                {
                    DispatcherTimer time = new DispatcherTimer();
                    time.Interval = TimeSpan.FromSeconds(1);
                    time.Tick += timer_Tick;
                    time.Start();
                }
                else
                {
                    return;
                }
            });
         
            //node command**********************************************************
            int i = 0;
            int i1 = 0;         
            RebootNodeCommand = new RelayCommand<ToggleButton>((o) => true, (o) =>
            {
                if (ConnectDeadState == false)
                {
                    if (o.IsChecked == true)
                    {
                        RebootNode = true;
                        RebootNodeUnchecked = false;
                        TimeRebootNode = ConvertDateTimeToTimestamp(DateTime.Now);
                    }
                    else
                    {
                        RebootNode = false;
                        RebootNodeUnchecked = true;
                        TimeRebootNodeUnchecked = ConvertDateTimeToTimestamp(DateTime.Now);
                    }
                }
            });

            RebirthNodeCommand = new RelayCommand<ToggleButton>((o) => true, (o) =>
            {
                if (ConnectDeadState == false)
                {
                    if (o.IsChecked == true)
                    {
                        RebirthNode = true;
                        RebirthNodeUnchecked = false;
                        TimeRebirthNode = ConvertDateTimeToTimestamp(DateTime.Now);
                    }
                    else
                    {
                        RebirthNode = false;
                        RebirthNodeUnchecked = true;
                        TimeRebirthNodeUnchecked = ConvertDateTimeToTimestamp(DateTime.Now);
                    }
                }
            });

              
            CntNodeCommand = new RelayCommand<ToggleButton>((o) => true, (o) =>
            {                
                if (o.IsChecked == true)
                {
                    TimeCntAndPublish = ConvertDateTimeToTimestamp(DateTime.Now);
                    seqBirth = i;
                    i++;
                    if (i > 255) { i = 0; }

                    // ## update metric reboot ##                
                    JsonRebootNBIRTH rebootNodeStr = new JsonRebootNBIRTH();
                    string str1 = rebootNodeStr.RebootNBIRTH("Node Control/Reboot",  TimeCntAndPublish, "Bool", RebootNode);

                    // ## update metric rebirth ##                             
                    JsonRebirthNBIRTH rebirthNodeStr = new JsonRebirthNBIRTH();
                    string str2 = rebirthNodeStr.RebirthNBIRTH("Node Control/Rebirth", TimeCntAndPublish, "Bool", RebirthNode);

                    // ## update metric scan rate ##                 
                    JsonScanNBIRTH scanNodeStr = new JsonScanNBIRTH();
                    string str3 = scanNodeStr.ScanNBIRTH("Node Control/Scan Rate", TimeCntAndPublish, "Int", ScanNodeValue);

                    // ## update metric supply voltage ##                 
                    JsonVoltNBIRTH voltNodeStr = new JsonVoltNBIRTH();
                    string str4 = voltNodeStr.VoltNBIRTH("Node Control/Supply Voltage", TimeCntAndPublish, "double", SupVoltValue);

                    // ## update metric hardware model ##
                    JsonHardwareNodeProperty hardwareNodeStr = new JsonHardwareNodeProperty();
                    string str5 = hardwareNodeStr.HardwareNBIRTH("Properties/Hardware Model", TimeCntAndPublish, "string", "Pi 3 Model B");

                    // ## update metric OS version ##
                    JsonOSNodeProperty OSNodeStr = new JsonOSNodeProperty();
                    string str6 = OSNodeStr.OSNBIRTH("Properties/OS Version", TimeCntAndPublish, "string", "Jessie with PIXEL/11.01.2017");

                    // ## update metric bdSeq ##
                    JsonBdSeq bdSeqStr = new JsonBdSeq();
                    string str0 = bdSeqStr.bdSeq("bdSeq", TimeCntAndPublish, "int", 0);

                    // ## update NBIRTH ##
                    string metrics = " " + str0 + " ," + str1 + " ," + str2 + " ," + str3+ " ," + str4 + " ," + str5+ " ," + str6  ;
                    NBIRTH nbth = new NBIRTH();
                    string mess = nbth.JsonNBIRTH( TimeCntAndPublish, metrics, seqBirth);

                    CurrentPayload = mess;
                    CurrentTopic = "spBv1.0/Demo/NBIRTH/RaspberryPi";

                    cntServer1.IsPublish = false;
                    if (cntServer1.PermitPublish == true)
                    {
                        cntServer1.IsPublish = true;
                        cntServer1.CntServerAndPublish(CurrentTopic, CurrentPayload);                                          
                    }
                    else
                    {
                        cntServer1.IsPublish = false;
                    }

                    ConnectDeadState = false;
                }

                else
                {
                    TimeDisCnt = ConvertDateTimeToTimestamp(DateTime.Now);

                    // ## update NDEATH ##
                    JsonBdSeq bdSeqStrDeath = new JsonBdSeq();
                    string strDeath = bdSeqStrDeath.bdSeq("bdSeq", TimeDisCnt, "int", 0);
                    NDEATH death = new NDEATH();
                    CurrentPayload = death.JsonNDEATH(TimeDisCnt, strDeath);
                    CurrentTopic = "spBv1.0/Demo/NDEATH/RaspberryPi";

                    cntServer1.IsPublish = false;
                    if (cntServer1.PermitPublish == true)
                    {
                        cntServer1.IsPublish = true;
                        cntServer1.CntServerAndPublish(CurrentTopic, CurrentPayload);  
                    }
                    else
                    {
                        cntServer1.IsPublish = false;
                    }
                    ConnectDeadState = true;
                }
                isTouched = true;
            });      

            PublishDataNodeCommand = new RelayCommand<Button>((o) => true, (o) =>
            {             
                if (ConnectDeadState == false)
                {
                    TimeDataPublish = ConvertDateTimeToTimestamp(DateTime.Now);
                    seqNDATA = i1;
                    i1++;
                    if (i1 > 255) { i1 = 0; }


                    // **NDATA
                    JsonVoltNBIRTH voltNodeStr = new JsonVoltNBIRTH();
                    string strNDATA = voltNodeStr.VoltNBIRTH("Node Control/Supply Voltage", TimeDataPublish, "double", SupVoltValue);
                    NDATA ndata = new NDATA();
                    CurrentPayload = ndata.JsonNDATA(TimeDataPublish, strNDATA, seqNDATA);
                    CurrentTopic = "spBv1.0/Demo/NDATA/RaspberryPi";

                    cntServer1.IsPublish = false;
                    if (cntServer1.PermitPublish == true)
                    {
                        cntServer1.IsPublish = true;
                        cntServer1.CntServerAndPublish(CurrentTopic, CurrentPayload);
                    }
                    else
                    {
                        cntServer1.IsPublish = false;
                    }                
                }
                isTouched = true;
            });

            PublishCmdNodeCommand = new RelayCommand<Button>((o) => true, (o) =>
            {
                if (ConnectDeadState == false)
                {
                    TimeCmdPublish = ConvertDateTimeToTimestamp(DateTime.Now);
                    // **NCMD
                    double currentRebootTime;
                    if (RebootNode == false && RebootNodeUnchecked == false)
                    {
                        currentRebootTime = TimeCmdPublish;
                    }
                    else if (RebootNodeUnchecked == true)
                    {
                        currentRebootTime = TimeRebootNodeUnchecked;
                    }
                    else
                    {
                        currentRebootTime = TimeRebootNode;
                    }
                    JsonRebootNBIRTH rebootNodeStr = new JsonRebootNBIRTH();
                    string str1 = rebootNodeStr.RebootNBIRTH("Node Control/Reboot", currentRebootTime, "Bool", RebootNode);


                    double currentRebirthTime;
                    if (RebirthNode == false && RebirthNodeUnchecked == false)
                    {
                        currentRebirthTime = TimeCmdPublish;
                    }
                    else if (RebirthNodeUnchecked == true)
                    {
                        currentRebirthTime = TimeRebirthNodeUnchecked;
                    }
                    else
                    {
                        currentRebirthTime = TimeRebirthNode;
                    }
                    JsonRebirthNBIRTH rebirthNodeStr = new JsonRebirthNBIRTH();
                    string str2 = rebirthNodeStr.RebirthNBIRTH("Node Control/Rebirth", currentRebirthTime, "Bool", RebirthNode);



                    JsonScanNBIRTH scanNodeStr = new JsonScanNBIRTH();
                    string str3 = scanNodeStr.ScanNBIRTH("Node Control/Scan Rate", TimeCmdPublish, "Int", ScanNodeValue);


                    NCMD ncmd = new NCMD();
                    string strNCMD = " " +str1 + " ," + str2 + " ," + str3;
                    CurrentPayload = ncmd.JsonNCMD(TimeCmdPublish, strNCMD);
                    CurrentTopic = "spBv1.0/Demo/NCMD/RaspberryPi";

                    cntServer1.IsPublish = false;
                    if (cntServer1.PermitPublish == true)
                    {
                        cntServer1.IsPublish = true;
                        cntServer1.CntServerAndPublish(CurrentTopic, CurrentPayload);
                    }
                    else
                    {
                        cntServer1.IsPublish = false;
                    }
                }
                isTouched = true;
            });


            //device command**********************************************************
            int k = 0;
            int k1 = 0;
            int k2 = 0;
            CntDevCommand = new RelayCommand<ToggleButton>((o) => true, (o) =>
            {             
                if (o.IsChecked == true)
                {
                    TimeCntAndPublishDev = ConvertDateTimeToTimestamp(DateTime.Now);
                    seqDBirth = k;
                    k++;
                    if (k > 255) { k = 0; }

                    JsonInput inp = new JsonInput();
                    string str0 = inp.InputDBIRTH("Input/A", TimeCntAndPublishDev, "bool", ValueInputChecked);

                    JsonOutput outp = new JsonOutput();
                    string str1 = outp.OutputDBIRTH("Output/B", TimeCntAndPublishDev, "bool", ValueOutput);

                    DeviceModel devmodel = new DeviceModel();
                    string str2 = devmodel.ModelDBIRTH("Properties", TimeCntAndPublishDev, "string", "Model A");

                    DBIRTH dbth = new DBIRTH();
                    string metrics = " " + str0 + " ," + str1 + " ," + str2;
                    string strDBIRTH = dbth.JsonDBIRTH(TimeCntAndPublishDev, metrics, seqDBirth);
                    CurrentPayload = strDBIRTH;
                    CurrentTopic = "spBv1.0/Demo/DBIRTH/RaspberryPi/Device A";

                    cntServer1.IsPublish = false;
                    if (cntServer1.PermitPublish == true)
                    {
                        cntServer1.IsPublish = true;
                        cntServer1.CntServerAndPublish(CurrentTopic, CurrentPayload);
                    }
                    else
                    {
                        cntServer1.IsPublish = false;
                    }

                    ConnectDeadStateDev = false;
                }
                else
                {
                    TimeDisCntDev = ConvertDateTimeToTimestamp(DateTime.Now);
                    seqDDeath = k1;
                    k1++;
                    if (k1 > 255) { k1 = 0; }

                    DDEATH ddth = new DDEATH();
                    string mess = ddth.JsonDDEATH(TimeDisCntDev, seqDDeath);
                    CurrentPayload = mess;
                    CurrentTopic = "spBv1.0/Demo/DDEATH/RaspberryPi/Device A";

                    cntServer1.IsPublish = false;
                    if (cntServer1.PermitPublish == true)
                    {
                        cntServer1.IsPublish = true;
                        cntServer1.CntServerAndPublish(CurrentTopic, CurrentPayload);
                    }
                    else
                    {
                        cntServer1.IsPublish = false;
                    }

                    ConnectDeadStateDev = true;                   
                }
                isTouched = true;
            });

            Input = new RelayCommand<ToggleButton>((o) => true, (o) =>
            {
                if (ConnectDeadStateDev == false)
                {
                    if (o.IsChecked == true)
                    {
                        ValueInputChecked = true;
                        ValueInputUnchecked = false;
                        TimeInputChecked = ConvertDateTimeToTimestamp(DateTime.Now);
                        ValueOutput = "true";
                    }
                    else
                    {
                        ValueInputChecked = false;
                        ValueInputUnchecked = true;
                        TimeInputUnchecked = ConvertDateTimeToTimestamp(DateTime.Now);
                        ValueOutput = "false";
                    }
                }
            });

            PublishDataDevCommand = new RelayCommand<Button>((o) => true, (o) =>
            {
                if (ConnectDeadStateDev == false)
                {
                    TimeDataDevPublish = ConvertDateTimeToTimestamp(DateTime.Now);
                    seqDDATA = k2;
                    k2++;
                    if (k2 > 255) { k2 = 0; }

                    // **DDATA
                    double currentPublishDataTime;
                    if (ValueInputChecked == false && ValueInputUnchecked == false)
                    {
                        currentPublishDataTime = TimeDataDevPublish;
                    }
                    else if (ValueInputUnchecked == true)
                    {
                        currentPublishDataTime = TimeInputUnchecked;
                    }
                    else
                    {
                        currentPublishDataTime = TimeInputChecked;
                    }

                    JsonInput inp = new JsonInput();
                    string str0 = inp.InputDBIRTH("Input/A", currentPublishDataTime , "bool", ValueInputChecked);
                    DDATA ddata = new DDATA();
                    CurrentPayload = ddata.JsonDDATA(TimeDataDevPublish, str0, seqDDATA);
                    CurrentTopic = "spBv1.0/Demo/DDATA/RaspberryPi/Device A";
                    
                    cntServer1.IsPublish = false;
                    if (cntServer1.PermitPublish == true)
                    {
                        cntServer1.IsPublish = true;
                        cntServer1.CntServerAndPublish(CurrentTopic, CurrentPayload);
                    }
                    else
                    {
                        cntServer1.IsPublish = false;
                    }                               
                }
                isTouched = true;
            });

            PublishCmdDevCommand = new RelayCommand<Button>((o) => true, (o) =>
            {          
                if (ConnectDeadStateDev == false)
                {
                    TimeCmdDevPublish = ConvertDateTimeToTimestamp(DateTime.Now);
                    JsonOutput outp = new JsonOutput();
                    string str1 = outp.OutputDBIRTH("Output/B", TimeCmdDevPublish, "bool", ValueOutput);
                    DCMD dcmd = new DCMD();
                    CurrentPayload = dcmd.JsonDCMD(TimeCmdDevPublish, str1);
                    CurrentTopic = "spBv1.0/Demo/DCMD/RaspberryPi/Device A";

                    cntServer1.IsPublish = false;
                    if (cntServer1.PermitPublish == true)
                    {
                        cntServer1.IsPublish = true;
                        cntServer1.CntServerAndPublish(CurrentTopic, CurrentPayload);
                    }
                    else
                    {
                        cntServer1.IsPublish = false;
                    }
                }
                isTouched = true;
            });

            // Subcribe*********************************************
            SubClick = new RelayCommand<Button>((o) => true, (o) =>
            {
                CurrentTopicSub = TopicSubcribe;
                MessageBox.Show("Subcribe success");
                
            });
        }

        bool PubConnect1Time = false;
        bool SubConnect1Time = false;

        public bool isTouched { get; set; }

        // connect server when loading      
        private async void timer_Tick(object sender, EventArgs e)
        {
            if (isTouched == true)
            {
                if (CurrentTopic == CurrentTopicSub)
                {
                    await cntServer2.CntServerAndSubcribe(CurrentTopicSub.Trim());
                    SplitStr();
                    isTouched = false;
                }
                else
                {
                    cntServer2.Message = "ERROR";
                    isTouched = false;
                }
            }

            if (SubConnect1Time == false)
            {
                bool a = await cntServer2.CntServerAndSubcribe("Start");
                if (a == true)
                {
                    SubConnect1Time = true;             
                }
                else
                {
                    SubConnect1Time = false;
                }
            }
                
                                    
            if (PubConnect1Time == false)
            {
                bool m = await cntServer1.CntServerAndPublish(CurrentTopic, CurrentPayload);
                if (m == true)
                {
                    cntServer1.PermitPublish = true;
                    PubConnect1Time = true;
                }
                else
                {
                    cntServer1.PermitPublish = false;
                    PubConnect1Time = false;
                }
            }      
        }

        public void SplitStr ()
        {
            if (cntServer2.Message != null)
            {
                string except = @" "",(){}:";
                string strResult = Regex.Replace(cntServer2.Message, @"[^a-zA-Z0-9" + except + "]+", string.Empty);
                CurrentMess = strResult;
            }
        }

    }
}


