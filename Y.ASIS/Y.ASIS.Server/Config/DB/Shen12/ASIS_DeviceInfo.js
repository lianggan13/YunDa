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
    Name: "L25音柱",
    Type: "Speaker",
    Ip: "10.60.25.8",
	Port: 5060,
    Extension: "{\"TerminalId\":2}"
}]);

db.getCollection("ASIS_DeviceInfo").insert([{
    ID: NumberInt("2"),
    Name: "L26音柱前",
    Type: "Speaker",
    Ip: "10.60.26.7",
    Port: 5060,
    Extension: "{\"TerminalId\":3}"
}]);
db.getCollection("ASIS_DeviceInfo").insert([{
    ID: NumberInt("3"),
    Name: "L26音柱后",
    Type: "Speaker",
    Ip: "10.60.26.8",
    Port: 5060,
    Extension: "{\"TerminalId\":4}"
}]);
db.getCollection("ASIS_DeviceInfo").insert([{
    ID: NumberInt("4"),
    Name: "L27音柱前",
    Type: "Speaker",
    Ip: "10.60.27.7",
    Port: 5060,
    Extension: "{\"TerminalId\":5}"
}]);
db.getCollection("ASIS_DeviceInfo").insert([{
    ID: NumberInt("5"),
    Name: "L27音柱后",
    Type: "Speaker",
    Ip: "10.60.27.8",
    Port: 5060,
    Extension: "{\"TerminalId\":6}"
}]);


















//---- 以下暂时保留备用 ----
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
