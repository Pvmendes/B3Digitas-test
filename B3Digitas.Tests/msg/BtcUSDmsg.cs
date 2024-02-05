﻿using Library.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace B3Digitas.Tests.msg
{
    public static class BtcUSDmsg
    {
        public static string msg
        {
            get
            {
                return "{\"data\":{\"timestamp\":\"1707136779\",\"microtimestamp\":\"1707136779090821\",\"bids\":[[\"43106\",\"0.00093612\"],[\"43103\",\"0.02274780\"],[\"43102\",\"0.34795680\"],[\"43101\",\"0.00825928\"],[\"43099\",\"0.01159917\"],[\"43098\",\"0.00880866\"],[\"43095\",\"0.02320437\"],[\"43094\",\"0.69614964\"],[\"43093\",\"1.02829500\"],[\"43092\",\"0.93987535\"],[\"43091\",\"0.38000000\"],[\"43087\",\"0.24370000\"],[\"43086\",\"0.69626942\"],[\"43085\",\"0.90580000\"],[\"43080\",\"2.26100411\"],[\"43079\",\"0.24370000\"],[\"43077\",\"0.69642185\"],[\"43074\",\"1.07270000\"],[\"43073\",\"0.51026657\"],[\"43071\",\"0.06247976\"],[\"43068\",\"0.94026993\"],[\"43064\",\"2.35770750\"],[\"43062\",\"0.24370000\"],[\"43053\",\"0.69681332\"],[\"43047\",\"0.24370000\"],[\"43046\",\"0.00919578\"],[\"43044\",\"0.55804245\"],[\"43043\",\"3.05589250\"],[\"43042\",\"0.24370000\"],[\"43041\",\"0.61218766\"],[\"43036\",\"0.61228248\"],[\"43033\",\"0.24370000\"],[\"43032\",\"2.95589250\"],[\"43030\",\"0.96726133\"],[\"43028\",\"0.24370000\"],[\"43025\",\"0.62240805\"],[\"43020\",\"0.61258613\"],[\"43019\",\"0.24370000\"],[\"43015\",\"0.61268107\"],[\"43014\",\"3.54770000\"],[\"43011\",\"0.00933147\"],[\"43010\",\"0.84527856\"],[\"43007\",\"0.24370000\"],[\"43005\",\"0.61287102\"],[\"43002\",\"1.00468579\"],[\"43001\",\"0.24370000\"],[\"43000\",\"0.61296604\"],[\"42998\",\"0.00093024\"],[\"42994\",\"0.97939043\"],[\"42992\",\"2.73860000\"],[\"42989\",\"0.85687519\"],[\"42988\",\"3.94010000\"],[\"42984\",\"0.61327029\"],[\"42981\",\"0.24370000\"],[\"42979\",\"0.61336542\"],[\"42978\",\"10.02010500\"],[\"42974\",\"0.61346057\"],[\"42972\",\"0.24370000\"],[\"42970\",\"4.97178771\"],[\"42969\",\"0.61130347\"],[\"42946\",\"0.24370000\"],[\"42928\",\"2.89846420\"],[\"42922\",\"0.24370000\"],[\"42920\",\"9.41300000\"],[\"42917\",\"0.24370000\"],[\"42913\",\"5.95491750\"],[\"42912\",\"0.24416696\"],[\"42851\",\"0.24370000\"],[\"42849\",\"4.14213750\"],[\"42845\",\"0.16679904\"],[\"42843\",\"0.24370000\"],[\"42829\",\"9.91600000\"],[\"42826\",\"0.00046837\"],[\"42801\",\"0.00373753\"],[\"42800\",\"0.23122160\"],[\"42794\",\"0.00046642\"],[\"42741\",\"0.00046431\"],[\"42716\",\"0.00046727\"],[\"42708\",\"0.00046735\"],[\"42701\",\"0.00060889\"],[\"42676\",\"2.07137250\"],[\"42655\",\"0.00047119\"],[\"42650\",\"0.00046799\"],[\"42638\",\"0.00116799\"],[\"42625\",\"0.01000000\"],[\"42610\",\"0.01000000\"],[\"42609\",\"0.01000000\"],[\"42607\",\"0.01000000\"],[\"42604\",\"0.01000000\"],[\"42602\",\"0.01000000\"],[\"42601\",\"0.00061031\"],[\"42600\",\"0.23244106\"],[\"42599\",\"0.01000000\"],[\"42598\",\"0.00046856\"],[\"42594\",\"0.01000000\"],[\"42591\",\"0.01000000\"],[\"42570\",\"0.00046710\"],[\"42550\",\"0.10000000\"],[\"42517\",\"0.00046945\"],[\"42501\",\"0.50061175\"]],\"asks\":[[\"43107\",\"1.65408234\"],[\"43108\",\"0.69593096\"],[\"43109\",\"0.26617314\"],[\"43110\",\"0.38000000\"],[\"43113\",\"0.09330075\"],[\"43114\",\"1.27199500\"],[\"43118\",\"3.55380043\"],[\"43120\",\"0.24370000\"],[\"43122\",\"0.06953572\"],[\"43123\",\"0.06961000\"],[\"43125\",\"0.03491416\"],[\"43127\",\"0.57999123\"],[\"43128\",\"0.69561783\"],[\"43129\",\"0.30494317\"],[\"43131\",\"0.24370000\"],[\"43132\",\"1.05500000\"],[\"43136\",\"1.80468000\"],[\"43137\",\"0.24370000\"],[\"43142\",\"0.17383931\"],[\"43143\",\"0.99700000\"],[\"43145\",\"0.84534223\"],[\"43151\",\"2.35816750\"],[\"43153\",\"0.24370000\"],[\"43154\",\"0.01000000\"],[\"43160\",\"0.29561659\"],[\"43164\",\"0.02000000\"],[\"43165\",\"2.07153883\"],[\"43166\",\"0.25370000\"],[\"43169\",\"0.23165234\"],[\"43170\",\"0.00223848\"],[\"43171\",\"0.24370000\"],[\"43172\",\"0.02000000\"],[\"43173\",\"2.95589250\"],[\"43175\",\"0.10223814\"],[\"43178\",\"0.24370000\"],[\"43180\",\"16.29602029\"],[\"43184\",\"3.19959250\"],[\"43185\",\"0.02000000\"],[\"43186\",\"0.73140127\"],[\"43189\",\"0.24370000\"],[\"43191\",\"0.60940228\"],[\"43194\",\"0.24370000\"],[\"43195\",\"0.26151361\"],[\"43196\",\"0.60930823\"],[\"43199\",\"0.24370000\"],[\"43201\",\"0.60921316\"],[\"43203\",\"3.30400000\"],[\"43204\",\"0.24370000\"],[\"43205\",\"4.01660000\"],[\"43206\",\"0.60911915\"],[\"43209\",\"0.24370000\"],[\"43212\",\"0.73481296\"],[\"43214\",\"0.05000000\"],[\"43217\",\"0.85261215\"],[\"43220\",\"0.16000000\"],[\"43222\",\"0.85251825\"],[\"43227\",\"0.85960436\"],[\"43232\",\"0.89233180\"],[\"43234\",\"0.73801865\"],[\"43238\",\"10.74765309\"],[\"43240\",\"0.24370000\"],[\"43243\",\"0.60619193\"],[\"43247\",\"4.94900000\"],[\"43248\",\"0.60609849\"],[\"43249\",\"0.24370000\"],[\"43250\",\"0.64228159\"],[\"43253\",\"0.60600507\"],[\"43254\",\"0.24370000\"],[\"43256\",\"0.00046228\"],[\"43257\",\"0.00400000\"],[\"43274\",\"0.24370000\"],[\"43283\",\"0.24620000\"],[\"43287\",\"0.01874707\"],[\"43289\",\"0.02934384\"],[\"43291\",\"0.24370000\"],[\"43297\",\"9.37800000\"],[\"43300\",\"0.00587853\"],[\"43301\",\"0.00060276\"],[\"43302\",\"5.95491750\"],[\"43325\",\"0.05000000\"],[\"43333\",\"0.05000000\"],[\"43335\",\"0.00500000\"],[\"43343\",\"0.00046629\"],[\"43367\",\"4.14213750\"],[\"43369\",\"0.01091572\"],[\"43380\",\"0.00227976\"],[\"43383\",\"0.00500000\"],[\"43400\",\"0.10883557\"],[\"43401\",\"0.00060137\"],[\"43429\",\"0.00045956\"],[\"43438\",\"0.15000000\"],[\"43439\",\"0.00500000\"],[\"43443\",\"0.30936212\"],[\"43444\",\"0.05000000\"],[\"43450\",\"0.00570035\"],[\"43458\",\"0.00111456\"],[\"43495\",\"0.05046215\"],[\"43500\",\"0.69940606\"],[\"43501\",\"0.00059999\"],[\"43510\",\"0.00700000\"]]},\"channel\":\"order_book_btcusd\",\"event\":\"data\"}";
            }
        }

        public static string Error_NoBidsAsksmsg
        {
            get
            {
                return "{\"data\":{\"timestamp\":\"1707136779\",\"microtimestamp\":\"1707136779090821\",\"bids\":[],\"asks\":[]},\"channel\":\"order_book_btcusd\",\"event\":\"data\"}";
            }
        }
        public static string Error_In_msg
        {
            get
            {
                return @"{'data':{'timestamp':'1707136779','microtimestamp':'1707136779090821',
                                    'bids':[['43106','0.00093612'],['43103','0.02274780']],
                                    'asks':[['43106','0.00093612'],['43103','0.02274780']]
                                },'channel':'order_book_btcusd','event':'data'}"
                ;
            }
        }
        public static string currencyPairDescription
        {
            get
            {
                return "BTC/USD";
            }
        }
    }
}
