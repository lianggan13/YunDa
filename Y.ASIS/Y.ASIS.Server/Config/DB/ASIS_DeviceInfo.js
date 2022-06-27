/*
 Navicat Premium Data Transfer

 Source Server         : localhost
 Source Server Type    : MongoDB
 Source Server Version : 40400
 Source Host           : localhost:27017
 Source Schema         : ASIS

 Target Server Type    : MongoDB
 Target Server Version : 40400
 File Encoding         : 65001

 Date: 30/05/2022 14:41:59
*/


// ----------------------------
// Collection structure for ASIS_DeviceInfo
// ----------------------------
db.getCollection("ASIS_DeviceInfo").drop();
db.createCollection("ASIS_DeviceInfo");

// ----------------------------
// Documents of ASIS_DeviceInfo
// ----------------------------
db.getCollection("ASIS_DeviceInfo").insert([ {
    ID: NumberInt("1"),
    Name: "6股道音柱#1",
    Type: "Speaker",
    Ip: "10.6.1.26",
	Port: 5060,
    Extension: "{\"TerminalId\":1}"
} ]);
db.getCollection("ASIS_DeviceInfo").insert([ {
    ID: NumberInt("2"),
    Name: "7股道音柱#2",
    Type: "Speaker",
    Ip: "10.6.1.27",
	Port: 5060,
    Extension: "{\"TerminalId\":2}"
} ]);

db.getCollection("ASIS_DeviceInfo").insert([ {
    ID: NumberInt("3"),
    Name: "8股道音柱",
    Type: "Speaker",
    Ip: "10.6.1.51",
	Port: 5060,
    Extension: "{\"TerminalId\":3}"
} ]);
db.getCollection("ASIS_DeviceInfo").insert([ {
    ID: NumberInt("4"),
    Name: "9股道音柱",
    Type: "Speaker",
    Ip: "10.6.1.52",
	Port: 5060,
    Extension: "{\"TerminalId\":4}"
} ]);

db.getCollection("ASIS_DeviceInfo").insert([ {
    ID: NumberInt("5"),
    Name: "10股道音柱#5",
    Type: "Speaker",
    Ip: "10.6.1.79",
	Port: 5060,
    Extension: "{\"TerminalId\":5}"
} ]);
db.getCollection("ASIS_DeviceInfo").insert([ {
    ID: NumberInt("6"),
    Name: "11股道音柱#6",
    Type: "Speaker",
    Ip: "10.6.1.80",
	Port: 5060,
    Extension: "{\"TerminalId\":6}"
} ]);


db.getCollection("ASIS_DeviceInfo").insert([ {
    _id: ObjectId("62860515057b00008a000ab6"),
    ID: NumberInt("997"),
    Name: "刷脸机(销权)",
    Type: "Attendance",
    Ip: "10.6.1.126",
    Extension: "{\"Type\":\"Revoke\"}"
} ]);

db.getCollection("ASIS_DeviceInfo").insert([ {
    _id: ObjectId("62860515057b00008a000ab8"),
    ID: NumberInt("998"),
    Name: "股道切换板卡#1",
    Type: "Switcher",
    Ip: "192.168.2.51",
    Port: 502,
    Extension: "{\"StartIndex\":1, \"Count\":6}"
} ]);
db.getCollection("ASIS_DeviceInfo").insert([ {
    _id: ObjectId("62860515057b00008a000ab9"),
    ID: NumberInt("999"),
    Name: "股道切换板卡#2",
    Type: "Switcher",
    Ip: "192.168.2.52",
    Port: 502,
    Extension: "{\"StartIndex\":1, \"Count\":6}"
} ]);
