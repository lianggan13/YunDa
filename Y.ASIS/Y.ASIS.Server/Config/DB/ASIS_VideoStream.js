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

 Date: 27/05/2022 16:40:48
*/


// ----------------------------
// Collection structure for ASIS_VideoStream
// ----------------------------
db.getCollection("ASIS_VideoStream").drop();
db.createCollection("ASIS_VideoStream");

// ----------------------------
// Documents of ASIS_VideoStream
// ----------------------------


// 6道-车号(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 1,
    Name: "6道-车号(非隔离开关侧)", 
    Type: "TrainNo",
    Channel: 1,
    Ip: "10.6.1.131",
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":0}",
} ]);

// 6道-验电
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 2,
    Name: "6道-验电", 
    Type: "Elec",
    Channel: 2,
    Ip: "10.6.1.147",
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 6道-接地
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 3,
    Name: "6道-接地", 
    Type: "Grounding",
	Ip: "10.6.1.148",
    Channel: 27,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 6道-通道南
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 4,
    Name: "6道-通道南", 
    Type: "Platform",
	Ip: "10.6.1.148",
    Channel: 3,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 6道-1车受电弓1
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 5,
    Name: "6道-1车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 4,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 6道-1车受电弓2
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 6,
    Name: "6道-1车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 5,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 6道-2车受电弓1 
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 7,
    Name: "6道-2车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 6,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 6道-2车受电弓2
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 8,
    Name: "6道-2车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 27,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 6道-通道北
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 9,
    Name: "6道-通道北", 
    Type: "Platform",
	Ip: "10.6.1.139",
    Channel: 8,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 6道-隔离开关
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 10,
    Name: "6道-隔离开关", 
    Type: "Isolation",
	Ip: "10.6.1.200",
    Channel: 82,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 6道-门禁枪机(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 11,
    Name: "6道-门禁枪机(隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.141",
    Channel: 9,
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":1}",
} ]);

// 6道-门禁枪机(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 12,
    Name: "6道-门禁枪机(非隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.142",
	Channel: 10,    
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":0}",
} ]);

// 6道-车号(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 13,
    Name: "6道-车号(隔离开关侧)", 
    Type: "TrainNo",
	Ip: "10.6.1.143",
    Channel: 11,
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":1}",
} ]);

// 6道-球机(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 14,
    Name: "6道-球机(隔离开关侧)", 
    Type: "UnKnow",
	Ip: "10.6.1.140",
    Channel: 12,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 6道-球机(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 15,
    Name: "6道-球机(非隔离开关侧)", 
    Type: "UnKnow",
	Ip: "10.6.1.140",
    Channel: 13,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);


// 7道-车号(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 16,
    Name: "7道-车号(非隔离开关侧)", 
    Type: "TrainNo",
	Ip: "10.6.1.146",
    Channel: 14,
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":0}",
} ]);


// 7道-验电
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 17,
    Name: "7道-验电", 
    Type: "Elec",
	Ip: "10.6.1.132",
    Channel: 15,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 7道-接地
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 18,
    Name: "7道-接地", 
    Type: "Grounding",
	Ip: "10.6.1.133",
    Channel: 16,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 7道-通道南
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 19,
    Name: "7道-通道南", 
    Type: "Platform",
	Ip: "10.6.1.148",
    Channel: 17,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 7道-1车受电弓1
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 20,
    Name: "7道-1车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 18,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 7道-1车受电弓2
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 21,
    Name: "7道-1车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 19,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 7道-2车受电弓1 
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 22,
    Name: "7道-2车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 20,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 7道-2车受电弓2 
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 23,
    Name: "7道-2车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 21,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 7道-通道北
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 24,
    Name: "7道-通道北", 
    Type: "Platform",
	Ip: "10.6.1.154",
    Channel: 22,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 7道-隔离开关
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 25,
    Name: "7道-隔离开关", 
    Type: "Isolation",
	Ip: "10.6.1.155",
    Channel: 23,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 7道-门禁枪机(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 26,
    Name: "7道-门禁枪机(隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.156",
    Channel: 24,
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":1}",
} ]);

// 7道-门禁枪机(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 27,
    Name: "7道-门禁枪机(非隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.157",
    Channel: 25,
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":0}",
} ]);

// 7道-车号(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 28,
    Name: "7道-车号(隔离开关侧)", 
    Type: "TrainNo",
	Ip: "10.6.1.158",
    Channel: 26,
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":1}",
} ]);


// 10道-车号(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 29,
    Name: "10道-车号(非隔离开关侧)", 
    Type: "TrainNo",
    Channel: 28,
    Ip: "10.6.1.131",
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":0}",
} ]);

// 10道-验电
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 30,
    Name: "10道-验电", 
    Type: "Elec",
    Channel: 29,
    Ip: "10.6.1.147",
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 10道-接地
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 31,
    Name: "10道-接地", 
    Type: "Grounding",
	Ip: "10.6.1.148",
    Channel: 30,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 10道-通道南
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 32,
    Name: "10道-通道南", 
    Type: "Platform",
	Ip: "10.6.1.148",
    Channel: 31,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 10道-1车受电弓1
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 33,
    Name: "10道-1车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 32,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 10道-1车受电弓2
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 34,
    Name: "10道-1车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 33,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 10道-2车受电弓1 
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 35,
    Name: "10道-2车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 34,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 10道-2车受电弓2
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 36,
    Name: "10道-2车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 35,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 10道-通道北
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 37,
    Name: "10道-通道北", 
    Type: "Platform",
	Ip: "10.6.1.139",
    Channel: 36,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 10道-隔离开关
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 38,
    Name: "10道-隔离开关", 
    Type: "Isolation",
	Ip: "10.6.1.206",
    Channel: 83,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 10道-门禁枪机(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 39,
    Name: "10道-门禁枪机(隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.141",
    Channel: 37,
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":0}",
} ]);

// 10道-门禁枪机(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 40,
    Name: "10道-门禁枪机(非隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.142",
	Channel: 38,    
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":1}",
} ]);

// 10道-车号(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 41,
    Name: "10道-车号(隔离开关侧)", 
    Type: "TrainNo",
	Ip: "10.6.1.143",
    Channel: 39,
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":1}",
} ]);

// 10道-球机(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 42,
    Name: "10道-球机(隔离开关侧)", 
    Type: "UnKnow",
	Ip: "10.6.1.140",
    Channel: 40,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 10道-球机(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 43,
    Name: "10道-球机(非隔离开关侧)", 
    Type: "UnKnow",
	Ip: "10.6.1.140",
    Channel: 84,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);


// 11道-车号(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 44,
    Name: "11道-车号(非隔离开关侧)", 
    Type: "TrainNo",
	Ip: "10.6.1.146",
    Channel: 41,
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":0}",
} ]);


// 11道-验电
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 45,
    Name: "11道-验电", 
    Type: "Elec",
	Ip: "10.6.1.132",
    Channel: 42,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 11道-接地
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 46,
    Name: "11道-接地", 
    Type: "Grounding",
	Ip: "10.6.1.133",
    Channel: 43,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 11道-通道南
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 47,
    Name: "11道-通道南", 
    Type: "Platform",
	Ip: "10.6.1.148",
    Channel: 44,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 11道-1车受电弓1
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 48,
    Name: "11道-1车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 45,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 11道-1车受电弓2
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 49,
    Name: "11道-1车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 46,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 11道-2车受电弓1 
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 50,
    Name: "11道-2车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 47,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 11道-2车受电弓2 
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 51,
    Name: "11道-2车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 48,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 11道-通道北
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 52,
    Name: "11道-通道北", 
    Type: "Platform",
	Ip: "10.6.1.154",
    Channel: 49,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 11道-隔离开关
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 53,
    Name: "11道-隔离开关", 
    Type: "Isolation",
	Ip: "10.6.1.155",
    Channel: 50,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 11道-门禁枪机(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 54,
    Name: "11道-门禁枪机(隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.156",
    Channel: 51,
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":0}",
} ]);

// 11道-门禁枪机(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 55,
    Name: "11道-门禁枪机(非隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.157",
    Channel: 52,
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":1}",
} ]);

// 11道-车号(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 56,
    Name: "11道-车号(隔离开关侧)", 
    Type: "TrainNo",
	Ip: "10.6.1.158",
    Channel: 53,
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":1}",
} ]);


// 8道-车号(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 57,
    Name: "8道-车号(非隔离开关侧)", 
    Type: "TrainNo",
    Channel: 54,
    Ip: "10.6.1.131",
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":0}",
} ]);

// 8道-验电
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 58,
    Name: "8道-验电", 
    Type: "Elec",
    Channel: 55,
    Ip: "10.6.1.147",
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 8道-接地
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 59,
    Name: "8道-接地", 
    Type: "Grounding",
	Ip: "10.6.1.148",
    Channel: 56,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 8道-通道南
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 60,
    Name: "8道-通道南", 
    Type: "Platform",
	Ip: "10.6.1.148",
    Channel: 57,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 8道-1车受电弓1
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 61,
    Name: "8道-1车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 58,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 8道-1车受电弓2
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 62,
    Name: "8道-1车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 59,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 8道-2车受电弓1 
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 63,
    Name: "8道-2车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 60,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 8道-2车受电弓2
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 64,
    Name: "8道-2车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 61,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 8道-通道北
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 65,
    Name: "8道-通道北", 
    Type: "Platform",
	Ip: "10.6.1.139",
    Channel: 62,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 8道-隔离开关
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 66,
    Name: "8道-隔离开关", 
    Type: "Isolation",
	Ip: "10.6.1.200",
    Channel: 63,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 8道-门禁枪机(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 67,
    Name: "8道-1号门禁枪机(隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.141",
    Channel: 64,
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":0}",
} ]);

// 8道-门禁枪机(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 68,
    Name: "8道-2号门禁枪机(非隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.142",
	Channel: 65,    
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":1}",
} ]);

// 8道-车号(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 69,
    Name: "8道-车号(隔离开关侧)", 
    Type: "TrainNo",
	Ip: "10.6.1.143",
    Channel: 66,
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":1}",
} ]);

// 8道-球机(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 70,
    Name: "8道-球机(隔离开关侧)", 
    Type: "UnKnow",
	Ip: "10.6.1.140",
    Channel: 67,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);


// 9道-车号(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 72,
    Name: "9道-车号(非隔离开关侧)", 
    Type: "TrainNo",
	Ip: "10.6.1.146",
    Channel: 68,
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":0}",
} ]);


// 9道-验电
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 73,
    Name: "9道-验电", 
    Type: "Elec",
	Ip: "10.6.1.132",
    Channel: 69,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 9道-接地
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 74,
    Name: "9道-接地", 
    Type: "Grounding",
	Ip: "10.6.1.133",
    Channel: 70,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 9道-通道南
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 75,
    Name: "9道-通道南", 
    Type: "Platform",
	Ip: "10.6.1.148",
    Channel: 71,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 9道-1车受电弓1
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 76,
    Name: "9道-1车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 72,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 9道-1车受电弓2
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 77,
    Name: "9道-1车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 73,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 9道-2车受电弓1 
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 78,
    Name: "9道-2车受电弓1", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 74,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);
// 9道-2车受电弓2 
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 79,
    Name: "9道-2车受电弓2", 
    Type: "Pantograph",
	Ip: "10.6.1.148",
    Channel: 75,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 9道-通道北
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 80,
    Name: "9道-通道北", 
    Type: "Platform",
	Ip: "10.6.1.154",
    Channel: 76,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 9道-隔离开关
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 81,
    Name: "9道-隔离开关", 
    Type: "Isolation",
	Ip: "10.6.1.155",
    Channel: 77,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);

// 9道-门禁枪机(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 82,
    Name: "9道-1号门禁枪机(隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.156",
    Channel: 78,
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":0}",
} ]);

// 9道-门禁枪机(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 83,
    Name: "9道-2号门禁枪机(非隔离开关侧)", 
    Type: "Door",
	Ip: "10.6.1.157",
    Channel: 79,
    Model: "HIK",
	Url: "",
	Extension: "{\"DoorIndex\":1}",
} ]);

// 9道-车号(隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 84,
    Name: "9道-车号(隔离开关侧)", 
    Type: "TrainNo",
	Ip: "10.6.1.158",
    Channel: 80,
    Model: "HIK",
	Url: "",
	Extension: "{\"TrainIndex\":1}",
} ])

// 8道-球机(非隔离开关侧)
db.getCollection("ASIS_VideoStream").insert([ {
    ID: 85,
    Name: "9道-球机(非隔离开关侧)", 
    Type: "UnKnow",
	Ip: "10.6.1.140",
    Channel: 81,
    Model: "HIK",
	Url: "",
	Extension: "",
} ]);












// DaHua
db.getCollection("ASIS_VideoStream").insert([ {
    _id: ObjectId("6284a64ecc58000026005a55"),
    ID: 99,
    Name: "隔离开关(大华)",
    Type: "Isolation",
	Ip: "192.168.1.109",
    Port: 37777,
	Channel: 0,
	UserName: "admin",
    Password: "yunda123",
    Model: "DaHua",
    Url: "rtsp://admin:yunda123@192.168.1.109:554/cam/realmonitor?channel=1&subtype=1",
	Extension: "",
} ]);

