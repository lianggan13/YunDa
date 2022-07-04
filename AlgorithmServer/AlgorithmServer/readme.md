运行环境: RTX3060 + CUDA11 + Windows10
(参考: https://blog.csdn.net/qq_43631789/article/details/116641664)

1.双击 cuda_11.0.2_451.48_win10.exe 安装(默认下一步)
2.安装后，解压 cudnn-11.0-windows-x64-v8.1.1.33
3.解压后，将解压后的文件，拷贝至安装路径(C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v11.0)
4.拷贝后，解压 TensorRT-7.2.3.4.Windows10.x86_64.cuda-11.0.cudnn8.1
5.解压后，将解压后 lib 文件夹中的 dll 文件，拷贝值 cuda bin 文件夹中

6.系统变量中加入下面的路径
	CUDA_BIN_PATH: %CUDA_PATH%\bin
	CUDA_LIB_PATH: %CUDA_PATH%\lib\x64
	CUDA_SDK_PATH: C:\ProgramData\NVIDIA Corporation\CUDA Samples\v11.0
	CUDA_SDK_BIN_PATH: %CUDA_SDK_PATH%\bin\win64
	CUDA_SDK_LIB_PATH: %CUDA_SDK_PATH%\common\lib\x64

7.系统变量path中加入下面的的变量
	%CUDA_BIN_PATH%
	%CUDA_LIB_PATH%
	%CUDA_SDK_BIN_PATH%
	%CUDA_SDK_LIB_PATH%

8.打开cmd输入nvcc -V
	cd C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v11.0\extras\demo_suite
	执行 bandwidthTest.exe
	执行 deviceQuery.exe

9.清理项目 --> 重新生成