/*
 Navicat Premium Data Transfer

 Source Server         : 127.0.0.1
 Source Server Type    : MongoDB
 Source Server Version : 40414
 Source Host           : localhost:27017
 Source Schema         : ASIS

 Target Server Type    : MongoDB
 Target Server Version : 40414
 File Encoding         : 65001

 Date: 07/07/2022 16:11:23
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
    _id: ObjectId("62c13a2cbe7900000a007c82"),
    ID: NumberInt("1"),
    Name: "L25音柱后",
    Type: "Speaker",
    Ip: "10.60.25.8",
    Port: 5060,
    Extension: "{\"TerminalId\":2}"
} ]);
db.getCollection("ASIS_DeviceInfo").insert([ {
    _id: ObjectId("62c13a2cbe7900000a007c83"),
    ID: NumberInt("2"),
    Name: "L26音柱前",
    Type: "Speaker",
    Ip: "10.60.26.7",
    Port: 5060,
    Extension: "{\"TerminalId\":3}"
} ]);
db.getCollection("ASIS_DeviceInfo").insert([ {
    _id: ObjectId("62c13a2cbe7900000a007c84"),
    ID: NumberInt("3"),
    Name: "L26音柱后",
    Type: "Speaker",
    Ip: "10.60.26.8",
    Port: 5060,
    Extension: "{\"TerminalId\":4}"
} ]);
db.getCollection("ASIS_DeviceInfo").insert([ {
    _id: ObjectId("62c13a2cbe7900000a007c85"),
    ID: NumberInt("4"),
    Name: "L27音柱前",
    Type: "Speaker",
    Ip: "10.60.27.7",
    Port: 5060,
    Extension: "{\"TerminalId\":5}"
} ]);
db.getCollection("ASIS_DeviceInfo").insert([ {
    _id: ObjectId("62c13a2cbe7900000a007c86"),
    ID: NumberInt("5"),
    Name: "L27音柱后",
    Type: "Speaker",
    Ip: "10.60.27.8",
    Port: 5060,
    Extension: "{\"TerminalId\":6}"
} ]);
db.getCollection("ASIS_DeviceInfo").insert([ {
    ID: NumberInt("6"),
    Name: "L25音柱前",
    Type: "Speaker",
    Ip: "10.60.25.7",
    Port: 5060,
    Extension: "{\"TerminalId\":1}"
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
