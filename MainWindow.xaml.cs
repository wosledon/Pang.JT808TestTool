using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Confluent.Kafka;
using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using Microsoft.Extensions.Logging.Abstractions;
using PMPlatform.JT808TestTool;
using PMPlatform.KafkaPubSubs;
using PMPlatform.KafkaPubSubs.JT808;
using PMPlatform.RelayProtocols;

namespace Pang.JT808TestTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JT808MsgIdProducer _provider;

        // 用于存储车辆在线情况
        protected ConcurrentDictionary<string, bool> SessionDictionary = new ConcurrentDictionary<string, bool>();
        protected ConcurrentDictionary<string, bool> SessionSelectDictionary = new ConcurrentDictionary<string, bool>();

        protected HashSet<JT808MsgId> HasSetMsgIds = new HashSet<JT808MsgId>()
        {
            JT808MsgId.位置信息汇报,
            JT808MsgId.终端通用应答,
            JT808MsgId.查询终端参数应答,
            JT808MsgId.位置信息查询应答
        };

        private bool _isShowMsg = true;

        public MainWindow()
        {
            InitializeComponent();

            InitConfig();
        }


        #region 遗弃

        //private Queue<JT808Package> JT808Queue;

        //static int count = 0;
        //private static int sum = 0;
        //void Init()
        //{
        //    var config = new ConsumerConfig
        //    {
        //        GroupId = "test-consumer-group",
        //        BootstrapServers = "127.0.0.1:9092",
        //        AutoOffsetReset = AutoOffsetReset.Latest,

        //        EnableAutoOffsetStore = false//<----this,

        //    };

        //    var consumer = new JT808MsgIdConsumer(config, new NullLoggerFactory());
        //    consumer.Subscribe();
        //    consumer.OnMessage((msg) =>
        //    {
        //        #region 暂留

        //        //Dispatcher.Invoke(() =>
        //        //{
        //        //    var package = new UpLinkRelayPackage().ToUpLinkRelayPackage(msg.data);
        //        //    var p = new JT808Serializer().Deserialize(package.Buffer);
        //        //    TbShowMsg.Text += $"[{DateTime.Now}]:[{Enum.GetName(package.Protocol)}协议]:[{msg.MsgId}]:{JsonConvert.SerializeObject(p).ToString()}" + Environment.NewLine;
        //        //    TbShowMsg.ScrollToEnd();
        //        //    count++;
        //        //    TbkReceiveCount.Content = (++sum).ToString();

        //        //    if (p.Header.MsgId.Equals(JT808MsgId.查询终端参数应答))
        //        //    {
        //        //        TbkShow.Text = JsonConvert.SerializeObject(p);
        //        //    }

        //        //    if (count > 20)
        //        //    {
        //        //        TbShowMsg.Clear();
        //        //        count = 0;
        //        //    }
        //        //});

        //        #endregion

        //        Dispatcher.Invoke(() =>
        //        {
        //            //var package = new UpLinkRelayPackage().ToUpLinkRelayPackage(msg.data);
        //            //var p = new JT808Serializer().Deserialize(package.Buffer);
        //            //TbShowMsg.Text += $"[{DateTime.Now}]:[{Enum.GetName(package.Protocol)}协议]:[{msg.MsgId}]:{JsonConvert.SerializeObject(p).ToString()}" + Environment.NewLine;
        //            //TbShowMsg.ScrollToEnd();
        //            //count++;
        //            //TbkReceiveCount.Content = (++sum).ToString();

        //            //if (p.Header.MsgId.Equals(140))
        //            //{
        //            //    TbkShow.Text = JsonConvert.SerializeObject(p);
        //            //}

        //            //if (count > 20)
        //            //{
        //            //    TbShowMsg.Clear();
        //            //    count = 0;
        //            //}
        //        });
        //    });


        //    Thread.Sleep(1000);
        //}

        //private void BtnReceiveConsumer_OnClick(object sender, RoutedEventArgs e)
        //{
        //    Init();
        //}

        //private void BtnSelectTerPara_OnClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var pNo = TbPhoneNo.Text;
        //        JT808Package jT808Package =
        //            JT808MsgId.查询终端参数.Create(pNo,
        //                new JT808_0x8104());
        //        var buffer = new JT808Serializer().Serialize(jT808Package, JT808Version.JTT2013);
        //        var str = buffer.ToHexString();

        //        _provider.ProduceAsync("jt808down", jT808Package.Header.MsgId.ToString(), pNo, buffer);
        //        _provider.Flush();
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception);
        //        throw;
        //    }
        //}

        //private void BtnServerParaSet_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var pNo = TbPhoneNo.Text;
        //    var body = new JT808_0x8103
        //    {
        //        ParamList = new List<JT808_0x8103_BodyBase>()
        //        {
        //            new JT808_0x8103_0x0013() {ParamValue = "127.0.0.1"},
        //            new JT808_0x8103_0x0018() {ParamValue = 4040}
        //        }
        //    };
        //    var jt808Package = JT808MsgId.设置终端参数.Create(pNo, body);

        //    var buffer = new JT808Serializer().Serialize(jt808Package, JT808Version.JTT2013);

        //    _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
        //    _provider.Flush();
        //}

        //private void BtnDisconnectOil_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var pNo = TbPhoneNo.Text;
        //    var body = new JT808_0x8105()
        //    {
        //        CommandWord = 0x64
        //    };
        //    var jt808Package = JT808MsgId.设置终端参数.Create(pNo, body);

        //    var buffer = new JT808Serializer().Serialize(jt808Package);

        //    _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
        //    _provider.Flush();
        //}

        //private void BtnSetReceiveTime_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var pNo = TbPhoneNo.Text;
        //    var body = new JT808_0x8103();
        //    body.ParamList = new List<JT808_0x8103_BodyBase>()
        //    {
        //        new JT808_0x8103_0x0027()
        //        {
        //            ParamValue = 5
        //        }
        //    };
        //    var jt808Package = JT808MsgId.设置终端参数.Create(pNo, body);

        //    var buffer = new JT808Serializer().Serialize(jt808Package);

        //    _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
        //    _provider.Flush();
        //}

        //private void BtnResetMileage_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var pNo = TbPhoneNo.Text;
        //    var body = new JT808_0x8103
        //    {
        //        ParamList = new List<JT808_0x8103_BodyBase>() {new JT808_0x8103_0x0080() {ParamValue = Convert.ToUInt32(TbMileage.Text) }}
        //    };
        //    var jt808Package = JT808MsgId.设置终端参数.Create(pNo, body);

        //    var buffer = new JT808Serializer().Serialize(jt808Package);

        //    _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
        //    _provider.Flush();
        //}

        //private void BtnSetMaxSpeed_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var pNo = TbPhoneNo.Text;
        //    var body = new JT808_0x8103
        //    {
        //        ParamList = new List<JT808_0x8103_BodyBase>() { new JT808_0x8103_0x0055() { ParamValue = Convert.ToUInt32(TbSpeed.Text) } }
        //    };
        //    var jt808Package = JT808MsgId.设置终端参数.Create(pNo, body);

        //    var buffer = new JT808Serializer().Serialize(jt808Package);

        //    _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
        //    _provider.Flush();
        //}

        //private void BtnSelectMileage_OnClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var pNo = TbPhoneNo.Text;
        //        JT808Package jt808Package = JT808MsgId.查询终端参数.Create(pNo,
        //            new JT808_0x8104_0x8106()
        //            {
        //                ParameterCount = 1,
        //                Parameters = new uint[] {0x0080}
        //            });
        //        var buffer = new JT808Serializer().Serialize(jt808Package, JT808Version.JTT2013);
        //        var str = buffer.ToHexString();

        //        _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
        //        _provider.Flush();
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception);
        //        throw;
        //    }
        //}

        //private void BtnSelectMaxSpeed_OnClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var pNo = TbPhoneNo.Text;
        //        JT808Package jt808Package = JT808MsgId.查询指定终端参数.Create(pNo,
        //            new JT808_0x8106()
        //            {
        //                ParameterCount = 1,
        //                Parameters = new uint[] {0x55}
        //            });
        //        var buffer = new JT808Serializer().Serialize(jt808Package, JT808Version.JTT2013);
        //        var str = buffer.ToHexString();

        //        _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
        //        _provider.Flush();
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception);
        //        throw;
        //    }
        //}

        //private void BtnSelectMaxSpeed0x8104_OnClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var pNo = TbPhoneNo.Text;
        //        JT808Package jt808Package = JT808MsgId.查询终端参数.Create(pNo,
        //            new JT808_0x8104_0x8106()
        //            {
        //                ParameterCount = 1,
        //                Parameters = new uint[] { 0x0055 }
        //            });
        //        var buffer = new JT808Serializer().Serialize(jt808Package, JT808Version.JTT2013);
        //        var str = buffer.ToHexString();

        //        _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
        //        _provider.Flush();
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception);
        //        throw;
        //    }
        //}

        #endregion

        private void InitConfig()
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    EquipmentCbxPaging.IsChecked = LocalConfigs.EquipmentPaging;
                    DataDisplayCbxShowLocationResponse.IsChecked = LocalConfigs.DataDisplayShowLocationResponse;
                    DataDisplayCbxShowTerminalGeneralResponse.IsChecked =
                        LocalConfigs.DataDisplayShowTerminalGeneralResponse;
                    DataDisplayCbxShowTerminalParaSelectResponse.IsChecked =
                        LocalConfigs.DataDisplayShowTerminalParaSelectResponse;
                    DataDisplayCbxShowTerminalLocationSelectResponse.IsChecked =
                        LocalConfigs.DataDisplayShowTerminalLocationSelectResponse;
                    DataDisplayCbxShowTerminalOthersResponse.IsChecked =
                        LocalConfigs.DataDisplayShowTerminalOthersResponse;
                    DataDisplayIsSerialization.IsChecked = LocalConfigs.DataDisplayIsSerialization;

                    ParaSetTbx0X0001.Text = LocalConfigs.ParaSet0X0001;
                    ParaSetTbx0X0010.Text = LocalConfigs.ParaSet0X0010;
                    ParaSetTbx0X0013.Text = LocalConfigs.ParaSet0X0013;
                    ParaSetTbx0X0017.Text = LocalConfigs.ParaSet0X0017;
                    ParaSetTbx0X0018.Text = LocalConfigs.ParaSet0X0018;
                    ParaSetTbx0X0020.Text = LocalConfigs.ParaSet0X0020;
                    ParaSetTbx0X0027.Text = LocalConfigs.ParaSet0X0027;
                    ParaSetTbx0X0029.Text = LocalConfigs.ParaSet0X0029;
                    ParaSetTbx0X0030.Text = LocalConfigs.ParaSet0X0030;
                    ParaSetTbx0X0055.Text = LocalConfigs.ParaSet0X0055;
                    ParaSetTbx0X0056.Text = LocalConfigs.ParaSet0X0056;
                    ParaSetTbx0X0080.Text = LocalConfigs.ParaSet0X0080;
                    ParaSetTbx0X0081.Text = LocalConfigs.ParaSet0X0081;
                    ParaSetTbx0X0082.Text = LocalConfigs.ParaSet0X0082;
                    ParaSetTbx0X0083.Text = LocalConfigs.ParaSet0X0083;
                    ParaSetTbx0X0084.Text = LocalConfigs.ParaSet0X0084;
                    ParaSetTbx0X1018.Text = LocalConfigs.ParaSet0X1018;

                    KafkaServerTbxConfig.Text = LocalConfigs.KafkaServer;

                    DataDisplayTbxShowMessageCount.Text = LocalConfigs.DataDisplayShowMessageCount;
                    DataDisplayTbkShowMaxMessagesCount.Text = LocalConfigs.DataDisplayMaxMessageCount;
                });
            });
        }

        private void InitKafkaMessageConsumer()
        {
            var server = KafkaServerTbxConfig.Text.Trim();
            //var groupId = Extensions.GenerateStringId();
            var groupId = "tool-consumer-group";

            var config = new ConsumerConfig
            {
                //GroupId = "test-consumer-group",
                GroupId = groupId,
                BootstrapServers = server,
                AutoOffsetReset = AutoOffsetReset.Latest,

                EnableAutoOffsetStore = false//<----this,

            };

            try
            {
                var consumer = new JT808MsgIdConsumer(config, new NullLoggerFactory());
                consumer.Subscribe();
                consumer.OnMessage((msg) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        var package = new UpLinkRelayPackage().ToUpLinkRelayPackage(msg.data);
                        var p = new JT808Serializer().Deserialize(package.Buffer);
                        bool isF = EquipmentCbxChooseAll.IsChecked != null && (bool) EquipmentCbxChooseAll.IsChecked;
                        EquipmentSpAdd(p.Header.TerminalPhoneNo, isF);
                        SessionDictionary.TryGetValue(p.Header.TerminalPhoneNo ?? string.Empty,
                            out var value);
                        if (value || LbCurrentEquipment.Content.Equals("全部"))
                        {
                            var json = package.Buffer.ToHexString();
                            if (DataDisplayIsSerialization.IsChecked != null && (bool)DataDisplayIsSerialization.IsChecked)
                            {
                                json = new JT808Serializer().Analyze(package.Buffer, options: new JsonWriterOptions()
                                {
                                    Indented = true,
                                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                                });
                            }

                            if (DataDisplayCbxShowLocationResponse.IsChecked != null && (bool)DataDisplayCbxShowLocationResponse.IsChecked)
                            {
                                if (p.Header.MsgId == (int)JT808MsgId.位置信息汇报)
                                {
                                    DataDisplaySpAdd(DataDisplaySpShowMessages, json);
                                }
                            }

                            if (DataDisplayCbxShowTerminalGeneralResponse.IsChecked != null &&
                                (bool)DataDisplayCbxShowTerminalGeneralResponse.IsChecked)
                            {
                                if (p.Header.MsgId == (int)JT808MsgId.终端通用应答)
                                {
                                    DataDisplaySpAdd(DataDisplaySpShowMessages, json);
                                }
                            }

                            if (DataDisplayCbxShowTerminalParaSelectResponse.IsChecked != null &&
                                (bool)DataDisplayCbxShowTerminalParaSelectResponse.IsChecked)
                            {
                                if (p.Header.MsgId == (int)JT808MsgId.查询终端参数应答)
                                {
                                    DataDisplaySpAdd(DataDisplaySpShowMessages, json);
                                }
                            }

                            if (DataDisplayCbxShowTerminalLocationSelectResponse.IsChecked != null &&
                                (bool)DataDisplayCbxShowTerminalLocationSelectResponse.IsChecked)
                            {
                                if (p.Header.MsgId == (int)JT808MsgId.位置信息查询应答)
                                {
                                    DataDisplaySpAdd(DataDisplaySpShowMessages, json);
                                }
                            }

                            if (DataDisplayCbxShowTerminalOthersResponse.IsChecked != null &&
                                (bool)DataDisplayCbxShowTerminalOthersResponse.IsChecked)
                            {
                                if (!HasSetMsgIds.TryGetValue((JT808MsgId)p.Header.MsgId, out var val))
                                {
                                    DataDisplaySpAdd(DataDisplaySpShowMessages, json);
                                }
                            }
                        }
                    });
                });

                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ChangeKafkaServerStatus(true);
                MessageBox.Show("请尝试重启该软件并重新连接Kafka!", "Kafka Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitKafkaSessionConsumer()
        {
            var server = KafkaServerTbxConfig.Text.Trim();
            var groupId = Extensions.GenerateStringId();

            var config = new ConsumerConfig
            {
                //GroupId = "test-consumer-group",
                GroupId = groupId,
                BootstrapServers = server,
                AutoOffsetReset = AutoOffsetReset.Latest,

                EnableAutoOffsetStore = false//<----this,

            };

            try
            {
                var consumer = new SessionPublishingConsumer(config, new NullLoggerFactory());
                consumer.Subscribe("sessionOffline");
                consumer.OnMessage((msg) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        var identify = Encoding.UTF8.GetString(msg.data);
                        SessionDictionary.TryRemove(identify, out var status);
                        foreach (CheckBox cBx in EquipmentSpList.Children)
                        {
                            if (cBx.Content.Equals(identify))
                            {
                                EquipmentSpList.Children.Remove(cBx);
                                if (LbCurrentEquipment.Content.Equals(cBx.Content))
                                {
                                    LbCurrentEquipment.Content = "暂时没有监听设备";
                                }
                                break;
                            }
                        }
                    });
                });

                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ChangeKafkaServerStatus(true);
                MessageBox.Show("请尝试重启该软件并重新连接Kafka!", "Kafka Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int _count = 0;
        private void DataDisplaySpAdd(StackPanel sp, string data)
        {
            var inputMax = Convert.ToInt32(DataDisplayTbxShowMessageCount.Text);
            var configMax = Convert.ToInt32(DataDisplayTbkShowMaxMessagesCount.Text);
            var maxMassageCount = inputMax < configMax ? inputMax : configMax; 

            if (sp.Children.Count > maxMassageCount)
            {
                sp.Children.RemoveRange(0, sp.Children.Count-maxMassageCount);
            }

            if (_isShowMsg)
            {
                sp.Children.Add(new DataDisplayControl(++_count, data));
                DataDisplaySvrSp.ScrollToEnd();
            }
        }

        private void EquipmentSpAdd(string key, bool value)
        {
            EquipmentTbkSumPages.Text = string.IsNullOrEmpty(EquipmentTbxSelect.Text)
                ? (SessionDictionary.Count / LocalConfigs.EquipmentPageSize +
                   (SessionDictionary.Count % LocalConfigs.EquipmentPageSize > 0 ? 1 : 0)).ToString()
                : (SessionSelectDictionary.Count / LocalConfigs.EquipmentPageSize +
                   (SessionSelectDictionary.Count % LocalConfigs.EquipmentPageSize > 0 ? 1 : 0)).ToString();

            if (SessionDictionary.TryAdd(key, value) && EquipmentSpList.Children.Count < LocalConfigs.EquipmentPageSize
                && SessionDictionary.Count <= LocalConfigs.EquipmentPageSize)
            {
                var item = new CheckBox()
                {
                    Content = key,
                    IsChecked = value,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(15, 5,5,0)
                };
                item.Click += (s, e) =>
                {
                    var thisItem = s as CheckBox;
                    var itemKey = thisItem?.Content.ToString();
                    if (thisItem?.IsChecked != null && (bool) thisItem.IsChecked)
                    {
                        if (SessionDictionary.TryGetValue(itemKey ?? string.Empty, out var itemValue))
                        {
                            SessionDictionary.TryUpdate(itemKey, (bool) thisItem.IsChecked, itemValue);
                        }

                        Dispatcher.Invoke(() =>
                        {
                            var chooseNum = SessionDictionary.Values.Count(x => x);
                            // ReSharper disable once PossibleNullReferenceException
                            LbCurrentEquipment.Content = chooseNum > 1
                                ? $"{SessionDictionary.Count}台设备"
                                : thisItem.Content;
                        });
                    }
                    else
                    {
                        EquipmentCbxChooseAll.IsChecked = false;

                        if (SessionDictionary.TryGetValue(itemKey ?? string.Empty, out var itemValue))
                        {
                            SessionDictionary.TryUpdate(itemKey, (bool)thisItem.IsChecked, itemValue);
                        }

                        Dispatcher.Invoke(() =>
                        {
                            var chooseNum = SessionDictionary.Values.Count(x => x);
                            // ReSharper disable once PossibleNullReferenceException
                            LbCurrentEquipment.Content = chooseNum > 1
                                ? $"{SessionDictionary.Count}台设备"
                                : chooseNum < 1? "暂时没有监听设备" : SessionDictionary.First(pair => pair.Value).Key;
                        });
                    }
                };
                EquipmentSpList.Children.Add(item);
            }
        }

        private void EquipmentSpAdd(StackPanel sp, ConcurrentDictionary<string, bool> dict, int begin)
        {
            sp.Children.Clear();
            foreach (var session in dict.Skip(begin).Take(LocalConfigs.EquipmentPageSize))
            {
                var key = session.Key;
                var value = session.Value;
                var item = new CheckBox()
                {
                    Content = key,
                    IsChecked = value,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(15, 5, 5, 0)
                };
                item.Click += (s, e) =>
                {
                    var thisItem = s as CheckBox;
                    var itemKey = thisItem?.Content.ToString();
                    if (thisItem?.IsChecked != null && (bool)thisItem.IsChecked)
                    {
                        if (dict.TryGetValue(itemKey ?? string.Empty, out var itemValue))
                        {
                            dict.TryUpdate(itemKey, (bool)thisItem.IsChecked, itemValue);
                        }

                        Dispatcher.Invoke(() =>
                        {
                            var chooseNum = SessionDictionary.Values.Count(x => x);
                            // ReSharper disable once PossibleNullReferenceException
                            LbCurrentEquipment.Content = chooseNum > 1
                                ? $"{SessionDictionary.Count}台设备"
                                : thisItem.Content;
                        });
                    }
                    else
                    {
                        EquipmentCbxChooseAll.IsChecked = false;

                        if (dict.TryGetValue(itemKey ?? string.Empty, out var itemValue))
                        {
                            dict.TryUpdate(itemKey, (bool)thisItem.IsChecked, itemValue);
                        }

                        Dispatcher.Invoke(() =>
                        {
                            var chooseNum = SessionDictionary.Values.Count(x => x);
                            // ReSharper disable once PossibleNullReferenceException
                            LbCurrentEquipment.Content = chooseNum > 1
                                ? $"{SessionDictionary.Count}台设备"
                                : chooseNum < 1 ? "暂时没有监听设备" : SessionDictionary.First(pair => pair.Value).Key;
                        });
                    }
                };
                sp.Children.Add(item);
            }
        }

        private void EquipmentCbxChooseAll_OnChecked(object sender, RoutedEventArgs e)
        {
            if (EquipmentCbxChooseAll.IsChecked != null && (bool)EquipmentCbxChooseAll.IsChecked)
            {
                foreach (var session in SessionDictionary)
                {
                    if (EquipmentCbxChooseAll.IsChecked != null)
                        SessionDictionary.TryUpdate(session.Key, (bool)EquipmentCbxChooseAll.IsChecked, session.Value);
                }
                foreach (CheckBox item in EquipmentSpList.Children)
                {
                    item.IsChecked = true;
                }

                LbCurrentEquipment.Content = "全部";
            }
            else
            {
                foreach (var session in SessionDictionary)
                {
                    if (EquipmentCbxChooseAll.IsChecked != null)
                        SessionDictionary.TryUpdate(session.Key, (bool)EquipmentCbxChooseAll.IsChecked, session.Value);
                }
                foreach (CheckBox item in EquipmentSpList.Children)
                {
                    item.IsChecked = false;
                }

                LbCurrentEquipment.Content = "暂时没有监听设备";
            }
        }

        private void ChangeKafkaServerStatus(bool status)
        {
            if (status)
            {
                KafkaServerBdrConnectStatus.Background = new SolidColorBrush(Colors.Green);
                KafkaServerBtnConnectTest.IsEnabled = false;
            }
            else
            {
                KafkaServerBdrConnectStatus.Background = new SolidColorBrush(Colors.Red);
                KafkaServerBtnConnectTest.IsEnabled = true;
            }
        }

        private void EquipmentBtnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            RequestClients();
        }

        private async void RequestClients()
        {
            var client = new HttpClient();

            var count = await client.GetStringAsync("http://localhost:5000/api/sessions/count");

            EquipmentBtnRefresh.Content = count;
        }

        private void EquipmentBtnSelect_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EquipmentTbxSelect.Text))
            {
                EquipmentSpAdd(EquipmentSpList, SessionDictionary, 0);
                return;
            }

            SessionSelectDictionary = new ConcurrentDictionary<string, bool>((from session in SessionDictionary
                where new Regex(EquipmentTbxSelect.Text.Trim()).IsMatch(session.Key)
                select session).ToDictionary(k=>k.Key, v=>v.Value));

            EquipmentSpAdd(EquipmentSpList, SessionSelectDictionary, 0);
        }

        private void EquipmentBtnPrevPage_OnClick(object sender, RoutedEventArgs e)
        {
            if (EquipmentTbkCurrentPage.Text.Equals("1"))
            {
                return;
            }

            EquipmentTbkCurrentPage.Text = (Convert.ToInt32(EquipmentTbkCurrentPage.Text) - 1).ToString();
            var prevPage = (Convert.ToInt32(EquipmentTbkCurrentPage.Text) - 1) * LocalConfigs.EquipmentPageSize;
            EquipmentSpAdd(EquipmentSpList,
                string.IsNullOrWhiteSpace(EquipmentTbxSelect.Text) ? SessionDictionary : SessionSelectDictionary,
                prevPage);
        }

        private void EquipmentBtnNextPage_OnClick(object sender, RoutedEventArgs e)
        {
            if (EquipmentTbkCurrentPage.Text.Equals(EquipmentTbkSumPages.Text))
            {
                return;
            }

            EquipmentTbkCurrentPage.Text = (Convert.ToInt32(EquipmentTbkCurrentPage.Text) + 1).ToString();
            var nextPage = (Convert.ToInt32(EquipmentTbkCurrentPage.Text)-1) * LocalConfigs.EquipmentPageSize;
            EquipmentSpAdd(EquipmentSpList, 
                string.IsNullOrWhiteSpace(EquipmentTbxSelect.Text) ? SessionDictionary : SessionSelectDictionary, 
                nextPage);
        }

        private void KafkaServerBtnConnectTest_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                InitKafkaMessageConsumer();
                InitKafkaSessionConsumer();
                _provider = new JT808MsgIdProducer(new ProducerConfig() { BootstrapServers = KafkaServerTbxConfig.Text });
                ChangeKafkaServerStatus(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kafka连接失败:{ex}", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                ChangeKafkaServerStatus(true);
            }
        }

        private void DataDisplayClearSpShowMessages_OnClick(object sender, RoutedEventArgs e)
        {
            DataDisplaySpShowMessages.Children.Clear();
            _count = 0;
        }

        private void DataDisplayPauseSpShowMessages_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataDisplayPauseSpShowMessages.Content.Equals("暂停"))
            {
                _isShowMsg = false;
                DataDisplayPauseSpShowMessages.Content = "继续";
            }
            else
            {
                _isShowMsg = true;
                DataDisplayPauseSpShowMessages.Content = "暂停";
            }
        }

        private void ParaSelectCbxChooseAll_OnClick(object sender, RoutedEventArgs e)
        {
            var root = sender as CheckBox;
            if (root?.IsChecked != null && (bool) root.IsChecked)
            {
                foreach (var cBox in ParaSelectSpCbxBox.Children)
                {
                    ((CheckBox)cBox).IsChecked = true;
                }
            }
            else
            {
                foreach (var cBox in ParaSelectSpCbxBox.Children)
                {
                    ((CheckBox)cBox).IsChecked = false;
                }
            }
        }

        private void ParaSelectBtnAll_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var pNo = LbCurrentEquipment.Content.ToString();
                if (pNo?.Length != 11)
                {
                    MessageBox.Show("性能考虑, 不支持批量查询!");
                    return;
                }
                JT808Package jT808Package =
                    JT808MsgId.查询终端参数.Create(pNo,
                        new JT808_0x8104());
                var buffer = new JT808Serializer().Serialize(jT808Package, JT808Version.JTT2013);
                var str = buffer.ToHexString();

                _provider.ProduceAsync("jt808down", jT808Package.Header.MsgId.ToString(), pNo, buffer);
                _provider.Flush();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void ParaSelectSpCbxBox_OnClick(object sender, RoutedEventArgs e)
        {
            if((e.Source as CheckBox)?.IsChecked == false){
                ParaSelectCbxChooseAll.IsChecked = false;
            }

            #region 丢弃

            //foreach (var item in SessionDictionary)
            //{
            //    if (item.Value)
            //    {
            //        var paras = new List<uint>();

            //        switch ((e.Source as CheckBox)?.Name)
            //        {
            //            case "ParaSelectCbx0X0001":
            //                paras.Add(0x0001);
            //                break;
            //            case "ParaSelectCbx0X0010":
            //                paras.Add(0x0010);
            //                break;
            //            case "ParaSelectCbx0X0013":
            //                paras.Add(0x0013);
            //                break;
            //            case "ParaSelectCbx0X0017":
            //                paras.Add(0x0017);
            //                break;
            //            case "ParaSelectCbx0X0018":
            //                paras.Add(0x0018);
            //                break;
            //            case "ParaSelectCbx0X0020":
            //                paras.Add(0x0020);
            //                break;
            //            case "ParaSelectCbx0X0027":
            //                paras.Add(0x0027);
            //                break;
            //            case "ParaSelectCbx0X0029":
            //                paras.Add(0x0029);
            //                break;
            //            case "ParaSelectCbx0X0030":
            //                paras.Add(0x0030);
            //                break;
            //            case "ParaSelectCbx0X0055":
            //                paras.Add(0x0055);
            //                break;
            //            case "ParaSelectCbx0X0056":
            //                paras.Add(0x0056);
            //                break;
            //            case "ParaSelectCbx0X0080":
            //                paras.Add(0x0080);
            //                break;
            //            case "ParaSelectCbx0X0082":
            //                paras.Add(0x0082);
            //                break;
            //            case "ParaSelectCbx0X0083":
            //                paras.Add(0x0083);
            //                break;
            //            case "ParaSelectCbx0X0084":
            //                paras.Add(0x0084);
            //                break;
            //            case "ParaSelectCbx0X1018":
            //                paras.Add(0x1018);
            //                break;
            //        }

            //        try
            //        {
            //            var pNo = item.Key;
            //            JT808Package jt808Package = JT808MsgId.查询指定终端参数.Create(pNo,
            //                new JT808_0x8106()
            //                {
            //                    ParameterCount = 1,
            //                    Parameters = paras.ToArray()
            //                });
            //            var buffer = new JT808Serializer().Serialize(jt808Package, JT808Version.JTT2013);
            //            var str = buffer.ToHexString();

            //            _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
            //            _provider.Flush();
            //        }
            //        catch (Exception exception)
            //        {
            //            Console.WriteLine(exception);
            //            throw;
            //        }
            //    }
            //}

            #endregion
        }

        private void ParaSelectBtnPart_OnClick(object sender, RoutedEventArgs e)
        {
            var paras = new List<uint>();
            foreach (var cBx in ParaSelectSpCbxBox.Children)
            {
                switch ((cBx as CheckBox)?.Name)
                {
                    case "ParaSelectCbx0X0001":
                        paras.Add(0x0001);
                        break;
                    case "ParaSelectCbx0X0010":
                        paras.Add(0x0010);
                        break;
                    case "ParaSelectCbx0X0013":
                        paras.Add(0x0013);
                        break;
                    case "ParaSelectCbx0X0017":
                        paras.Add(0x0017);
                        break;
                    case "ParaSelectCbx0X0018":
                        paras.Add(0x0018);
                        break;
                    case "ParaSelectCbx0X0020":
                        paras.Add(0x0020);
                        break;
                    case "ParaSelectCbx0X0027":
                        paras.Add(0x0027);
                        break;
                    case "ParaSelectCbx0X0029":
                        paras.Add(0x0029);
                        break;
                    case "ParaSelectCbx0X0030":
                        paras.Add(0x0030);
                        break;
                    case "ParaSelectCbx0X0055":
                        paras.Add(0x0055);
                        break;
                    case "ParaSelectCbx0X0056":
                        paras.Add(0x0056);
                        break;
                    case "ParaSelectCbx0X0080":
                        paras.Add(0x0080);
                        break;
                    case "ParaSelectCbx0X0082":
                        paras.Add(0x0082);
                        break;
                    case "ParaSelectCbx0X0083":
                        paras.Add(0x0083);
                        break;
                    case "ParaSelectCbx0X0084":
                        paras.Add(0x0084);
                        break;
                    case "ParaSelectCbx0X1018":
                        paras.Add(0x1018);
                        break;
                }
            }

            foreach (var session in SessionDictionary)
            {
                if (session.Value)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            var pNo = session.Key;
                            JT808Package jt808Package = JT808MsgId.查询指定终端参数.Create(pNo,
                                new JT808_0x8106()
                                {
                                    ParameterCount = 1,
                                    Parameters = paras.ToArray()
                                });
                            var buffer = new JT808Serializer().Serialize(jt808Package, JT808Version.JTT2013);
                            var str = buffer.ToHexString();

                            _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
                            _provider.Flush();
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            throw;
                        }
                    });
                }
            }
        }

        private void ParaSetCbxChooseAll_OnClick(object sender, RoutedEventArgs e)
        {
            if (ParaSetCbxChooseAll.IsChecked != null && (bool) ParaSetCbxChooseAll.IsChecked)
            {
                foreach (var item in ParaSetSpCbxBox.Children)
                {
                    if (item.GetType().UnderlyingSystemType.Name == "CheckBox")
                    {
                        ((CheckBox)item).IsChecked = true;
                    }
                }
            }
            else
            {
                foreach (var item in ParaSetSpCbxBox.Children)
                {
                    if (item.GetType().UnderlyingSystemType.Name == "CheckBox")
                    {
                        ((CheckBox)item).IsChecked = false;
                    }
                }
            }
        }

        private void ParaSetSpCbxBox_OnClick(object sender, RoutedEventArgs e)
        {
            if((e.Source as CheckBox)?.IsChecked == false){
                ParaSetCbxChooseAll.IsChecked = false;
            }
        }

        private void ParaSetBtnSet_OnClick(object sender, RoutedEventArgs e)
        {
            var body = new JT808_0x8103()
            {
                ParamList = new List<JT808_0x8103_BodyBase>()
            };

            foreach (var cBx in ParaSetSpCbxBox.Children)
            {
                if (cBx.GetType().UnderlyingSystemType.Name == "TextBox")
                {
                    continue;
                }
                if (((CheckBox)cBx).IsChecked == false)
                {
                    continue;
                }
                switch (((CheckBox)cBx)?.Name)
                {
                    case "ParaSetCbx0X0001":
                        body.ParamList.Add(new JT808_0x8103_0x0001()
                        {
                            ParamValue = Convert.ToUInt32(ParaSetTbx0X0001.Text.Trim())
                        });
                        break;
                    case "ParaSetCbx0X0010":
                        body.ParamList.Add(new JT808_0x8103_0x0010()
                        {
                            ParamValue = ParaSetTbx0X0010.Text.Trim()
                        });
                        break;
                    case "ParaSetCbx0X0013":
                        body.ParamList.Add(new JT808_0x8103_0x0013()
                        {
                            ParamValue = ParaSetTbx0X0013.Text.Trim()
                        });
                        break;
                    case "ParaSetCbx0X0017":
                        body.ParamList.Add(new JT808_0x8103_0x0017()
                        {
                            ParamValue = ParaSetTbx0X0017.Text.Trim()
                        });
                        break;
                    case "ParaSetCbx0X0018":
#pragma warning disable 618
                        body.ParamList.Add(new JT808_0x8103_0x0018()
#pragma warning restore 618
                        {
                            ParamValue = Convert.ToUInt32(ParaSetTbx0X0018.Text.Trim())
                        });
                        break;
                    case "ParaSetCbx0X0020":
                        body.ParamList.Add(new JT808_0x8103_0x0020()
                        {
                            ParamValue = Convert.ToUInt32(ParaSetTbx0X0020.Text.Trim())
                        });
                        break;
                    case "ParaSetCbx0X0027":
                        body.ParamList.Add(new JT808_0x8103_0x0027()
                        {
                            ParamValue = Convert.ToUInt32(ParaSetTbx0X0027.Text.Trim())
                        });
                        break;
                    case "ParaSetCbx0X0029":
                        body.ParamList.Add(new JT808_0x8103_0x0029()
                        {
                            ParamValue = Convert.ToUInt32(ParaSetTbx0X0029.Text.Trim())
                        });
                        break;
                    case "ParaSetCbx0X0030":
                        body.ParamList.Add(new JT808_0x8103_0x0030()
                        {
                            ParamValue = Convert.ToUInt32(ParaSetTbx0X0030.Text.Trim())
                        });
                        break;
                    case "ParaSetCbx0X0055":
                        body.ParamList.Add(new JT808_0x8103_0x0055()
                        {
                            ParamValue = Convert.ToUInt32(ParaSetTbx0X0055.Text.Trim())
                        });
                        break;
                    case "ParaSetCbx0X0056":
                        body.ParamList.Add(new JT808_0x8103_0x0056()
                        {
                            ParamValue = Convert.ToUInt32(ParaSetTbx0X0056.Text.Trim())
                        });
                        break;
                    case "ParaSetCbx0X0080":
                        body.ParamList.Add(new JT808_0x8103_0x0080()
                        {
                            ParamValue = Convert.ToUInt32(ParaSetTbx0X0080.Text.Trim())
                        });
                        break;
                    case "ParaSetCbx0X0082":
                        body.ParamList.Add(new JT808_0x8103_0x0082()
                        {
                            ParamValue = Convert.ToByte(ParaSetTbx0X0082.Text.Trim())
                        });
                        break;
                    case "ParaSetCbx0X0083":
                        body.ParamList.Add(new JT808_0x8103_0x0083()
                        {
                            ParamValue = ParaSetTbx0X0083.Text.Trim()
                        });
                        break;
                    case "ParaSetCbx0X0084":
                        body.ParamList.Add(new JT808_0x8103_0x0084()
                        {
                            ParamValue = Convert.ToByte(ParaSetTbx0X0084.Text.Trim())
                        });
                        break;
                    //case "ParaSetCbx0X1018":
                    //    paras.ParamList.Add(new JT808_0x8103_0x1018()
                    //    {
                    //        ParamValue = ParaSetTbx0X0010.Text.Trim()
                    //    });
                    //    break;
                }
            }

            foreach (var session in SessionDictionary)
            {
                Task.Run(() =>
                {
                    try
                    {
                        var pNo = session.Key;
                        var jt808Package = JT808MsgId.设置终端参数.Create(pNo, body);

                        var buffer = new JT808Serializer().Serialize(jt808Package);

                        _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), pNo, buffer);
                        _provider.Flush();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                });
            }
        }

        private void EquipmentControlSpOilEle_OnClick(object sender, RoutedEventArgs e)
        {
            JT808_0x8105 body;
            if (((Button) e.Source).Content.Equals("打开油电"))
            {
                ChangeButtonStatus(EquipmentOpenSpOilEle, true);
                ChangeButtonStatus(EquipmentCloseSpOilEle, false);

                body = new JT808_0x8105()
                {
                    CommandWord = 0x64
                };
            }
            else
            {
                ChangeButtonStatus(EquipmentOpenSpOilEle, false);
                ChangeButtonStatus(EquipmentCloseSpOilEle, true);

                body = new JT808_0x8105()
                {
                    CommandWord = 0x65
                };
            }

            foreach (var session in SessionDictionary)
            {
                if (session.Value)
                {
                    var jt808Package = JT808MsgId.终端控制.Create(session.Key, body);

                    var buffer = new JT808Serializer().Serialize(jt808Package);

                    _provider.ProduceAsync("jt808down", jt808Package.Header.MsgId.ToString(), session.Key, buffer);
                    _provider.Flush();
                }
            }
        }

        private void ChangeButtonStatus(Button btn, bool status)
        {
            if (status)
            {
                btn.Background = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                btn.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    LocalConfigs.EquipmentPaging = EquipmentCbxPaging.IsChecked ??= false;
                    LocalConfigs.DataDisplayShowLocationResponse =
                        DataDisplayCbxShowLocationResponse.IsChecked ??= false;
                    LocalConfigs.DataDisplayShowTerminalGeneralResponse =
                        DataDisplayCbxShowTerminalGeneralResponse.IsChecked ??= false;
                    LocalConfigs.DataDisplayShowTerminalParaSelectResponse =
                        DataDisplayCbxShowTerminalParaSelectResponse.IsChecked ??= false;
                    LocalConfigs.DataDisplayShowTerminalLocationSelectResponse =
                        DataDisplayCbxShowTerminalLocationSelectResponse.IsChecked ??= false;
                    LocalConfigs.DataDisplayShowTerminalOthersResponse =
                        DataDisplayCbxShowTerminalOthersResponse.IsChecked ??= false;
                    LocalConfigs.DataDisplayIsSerialization = DataDisplayIsSerialization.IsChecked ??= false;

                    LocalConfigs.ParaSet0X0001 = ParaSetTbx0X0001.Text;
                    LocalConfigs.ParaSet0X0010 = ParaSetTbx0X0010.Text;
                    LocalConfigs.ParaSet0X0013 = ParaSetTbx0X0013.Text;
                    LocalConfigs.ParaSet0X0017 = ParaSetTbx0X0017.Text;
                    LocalConfigs.ParaSet0X0018 = ParaSetTbx0X0018.Text;
                    LocalConfigs.ParaSet0X0020 = ParaSetTbx0X0020.Text;
                    LocalConfigs.ParaSet0X0027 = ParaSetTbx0X0027.Text;
                    LocalConfigs.ParaSet0X0029 = ParaSetTbx0X0029.Text;
                    LocalConfigs.ParaSet0X0030 = ParaSetTbx0X0030.Text;
                    LocalConfigs.ParaSet0X0055 = ParaSetTbx0X0055.Text;
                    LocalConfigs.ParaSet0X0056 = ParaSetTbx0X0056.Text;
                    LocalConfigs.ParaSet0X0080 = ParaSetTbx0X0080.Text;
                    LocalConfigs.ParaSet0X0081 = ParaSetTbx0X0081.Text;
                    LocalConfigs.ParaSet0X0082 = ParaSetTbx0X0082.Text;
                    LocalConfigs.ParaSet0X0083 = ParaSetTbx0X0083.Text;
                    LocalConfigs.ParaSet0X0084 = ParaSetTbx0X0084.Text;
                    LocalConfigs.ParaSet0X1018 = ParaSetTbx0X1018.Text;

                    LocalConfigs.KafkaServer = KafkaServerTbxConfig.Text;

                    LocalConfigs.DataDisplayShowMessageCount = DataDisplayTbxShowMessageCount.Text;
                    LocalConfigs.DataDisplayMaxMessageCount = DataDisplayTbkShowMaxMessagesCount.Text;
                });
            });
        }
    }
}
