namespace Y.ASIS.Server.Device
{
    public enum PLCNodeType
    {
        /// <summary>
        /// 系统状态 00=系统初始化中 01=设备正常 02=设备故障
        /// </summary>
        SystemState,

        /// <summary>
        /// 系统类型 00=无编组 01=单车8编组 02=重联8编组 03=单车16编组 其余非法
        /// </summary>
        SystemType,

        /// <summary>
        /// 系统时间 1970.01.01 到当前时刻的秒数
        /// </summary>
        SystemTime,

        /// <summary>
        /// 系统严重错误，需弹窗显示 Bit0：隔离开关状态错误 Bit1：验电装置状态错误或不匹配
        /// Bit2：接地装置状态错误或不匹配 Bit3：门禁状态错误或不匹配
        /// </summary>
        SystemError,

        /// <summary>
        /// 股道号
        /// </summary>
        TrackNo,

        /// <summary>
        /// 列位号 00=东列位 01=西列位
        /// </summary>
        PositionNo,

        /// <summary>
        /// 隔离开关状态 00=初始化检测中 01=合闸位置 02=分闸位置 03=合闸中 04=分闸中 10=故障状态
        /// </summary>
        IsolationState,

        /// <summary>
        /// 隔离开关故障码 00=通信失败 01=合闸位置失效 02=分闸位置失效 03=指令超时
        /// </summary>
        IsolationError,

        /// <summary>
        /// 接地开关状态 00=初始化检测中 01=挂接地位置 02=撤接地位置 03=挂接地中 04=撤接地中 10=故障状态
        /// </summary>
        GroundingState,

        /// <summary>
        /// 接地开关故障码 00=通信失败 01=挂接地位置1失效 02=挂接地位置2失效 03=挂接地位置失效 04=指令超时
        /// </summary>
        GroundingError,

        /// <summary>
        /// 验电装置状态 00=初始化检测中 01=验电位置 02=撤验电位置 03=挂验电中 04=撤验电中 10=故障状态
        /// </summary>
        ElecState,

        /// <summary>
        /// 验电结果 00=初始化检测中 01=验电有电 02=验电无电 10=故障状态
        /// </summary>
        ElecResult,

        /// <summary>
        /// 验电装置故障码 00=通信失败 01=验电位置失效 02=撤验电位置失效 03=验电失效 04=指令超时
        /// </summary>
        ElecError,

        /// <summary>
        /// LED显示屏状态 00=初始化检测中 01=合闸有电显示 02=分闸有电显示 03=无电显示 04=故障状态 11=未配置
        /// </summary>
        LEDState,

        /// <summary>
        /// 信号灯状态 00=初始化中 01=全熄灭 02=仅白灯亮 03=仅红灯亮 04=白灯、红灯均亮 11=未配置
        /// </summary>
        SignalLightState,

        /// <summary>
        /// 工具领用状态 00=初始化中 01=使用中 02=未使用 10=故障
        /// </summary>
        ToolState,

        /// <summary>
        /// 道闸机状态 00=初始化中 01=降杆位置 02=升杆位置 03=降杆中 04=升杆中 05=未知状态 11=未配置
        /// </summary>
        GateState,

        /// <summary>
        /// 车位状态 00=初始化中 01=有车 02=无车 03=线路串线 11=未配置 
        /// </summary>
        TrainState,

        /// <summary>
        /// 警示灯状态 00=初始化中 01=警示状态 02=未警示状态 03=故障状态 11=未配置
        /// </summary>
        WarningState,

        /// <summary>
        /// 操作模式 01=申请批复模式 02=远程模式
        /// </summary>
        OperationMode,

        /// <summary>
        /// 操作流程 01=断电流程 02=送电流程
        /// </summary>
        OperationFlow,

        /// <summary>
        /// 门禁状态 00=初始化检测中 01=使能状态 02=非使能状态 10=故障状态 11=未配置
        /// </summary>
        DoorState,

        /// <summary>
        /// 门锁状态 00=初始化检测中 01=有效关门 02有效开门
        /// </summary>
        DoorLocker,

        /// <summary>
        /// 门所在平台序号
        /// </summary>
        DoorIndex,

        /// <summary>
        /// 登顶人员信息 2021/2/21 18:16:30 123456
        /// ::AsGlobalPV:State.Person_info
        /// </summary>
        TopPersonInfo,

        /// <summary>
        /// 客流计计数 字符串，如“1”、“-2”、“Error”、“Null”，负号代表出门人数大于进门人数，“Error”代表设备错误。
        /// </summary>
        PassCount,

        /// <summary>
        /// 操作终端读头状态 00=初始化检测中 01=正常未验证(无卡号) 02=正常待验证 03=正常已验证 10=故障状态 11=未配置
        /// </summary>
        OperatorReaderState,

        /// <summary>
        /// 操作终端授权人员信息 2021/2/21 18:16:30 123456
        /// </summary>
        OperatorPersonInfo,

        /// <summary>
        /// 验电电压 22.3KV Error
        /// </summary>
        ElecValue,

        /// <summary>
        /// 接地电阻 3.2Ω Error
        /// </summary>
        GroundingResistance,

        /// <summary>
        /// 回路电阻 3.2Ω Error
        /// </summary>
        LoopResistance,

        /// <summary>
        /// 系统DI状态(包括按钮) 00=初始化检测中 01=有效1 02=有效0 03=无效
        /// </summary>
        SystemDIState,

        /// <summary>
        /// 系统DO状态 00=初始化检测中 01=有效1 02=有效0 03=无效
        /// </summary>
        SystemDOState,

        /// <summary>
        /// 渡板状态 00=初始化检测中 01=渡板伸出有效 02=渡板收回有效 03=故障状态
        /// </summary>
        GangwayState,

        /// <summary>
        /// 库门状态 00=初始化检测中 01=关门 02=开门 11=未配置
        /// </summary>
        DepotGateState,

        /// <summary>
        /// 当期操作记录读取状态 00=未读 01=已读
        /// </summary>
        OperationRecordState,

        /// <summary>
        /// 当前故障记录读取状态 00=未读 01=已读
        /// </summary>
        FaultRecordState,

        /// <summary>
        /// 当前门禁记录读取状态 00=未读 01=已读
        /// </summary>
        AccessRecordState,

        /// <summary>
        /// 操作记录 2020/02/02 19:26:20 123456 023 -> 2020年2月2日19:26:20操作员123456执行了操作码为023的操作
        /// </summary>
        OperationRecord,

        /// <summary>
        /// 故障记录 2020/02/02 19:26:20 001 002 -> 2020年2月2日19:26:20设备号为01的设备发生了故障码为002的故障
        /// </summary>
        FaultRecord,

        /// <summary>
        /// 门禁记录 2020/02/02 19:26:20 888888 001 -> 2020年2月2日19:26:20人员编号为为888888的人员进入门1（001进入门1,002进入门2，003门3进入门3类推；101出门1,102出门2；103出门3类推）
        /// </summary>
        AccessRecord,

        /// <summary>
        /// 操作指令
        /// </summary>
        OperationCommand,

        /// <summary>
        /// 操作申请
        /// </summary>
        OperationApply,

        /// <summary>
        /// 设备对位状态
        /// </summary>
        EnableReset,

        /// <summary>
        /// 安全确认
        /// </summary>
        SafeConfirm,

        /// <summary>
        /// 算法结果
        /// </summary>
        AlgorithmResult,

        /// <summary>
        /// 信号灯指令
        /// </summary>
        SignalLightCommand,

        /// <summary>
        /// 道闸机指令
        /// </summary>
        GateCommand,

        /// <summary>
        /// 操作终端授权人员信息 123456#888888##Url=123.png
        /// </summary>
        Operators,

        /// <summary>
        /// 登顶授权人员信息 123456#888888##Url=123.png
        /// </summary>
        Workers,

        /// <summary>
        /// 算法弹窗提示信息
        /// </summary>
        AlgorithmMsg,

        /// <summary>
        /// 巡检人员卡号
        /// </summary>
        ID_Inspect,

        /// <summary>
        /// 操作人 （张三，李四，王二）
        /// </summary>
        OperatorNames,

        /// <summary>
        /// 作业人 (张三，李四，王二)
        /// </summary>
        WorkerNames,

        /// <summary>
        /// 作业类型
        /// </summary>
        WorkType,
    }
}
