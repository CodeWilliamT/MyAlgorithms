import sys
from HTM_Bsp_Lib import *

def bsp_init():
    _para = INIT_PARA('paras.db'.encode(), 0, 1, 1, 10, 20, 3, 0, 0)
    err = HTM.Init(_para)
    print('Para file:', _para.para_file)
    print('Axis num:', _para.max_axis_num)
    print('I/O num:', _para.max_io_num)
    print('Run mode:', _para.offline_mode)
    print('Language:', _para.language)
    if err < 0:
        print('初始化失败，error=' + str(err))
    return err


def load_ui():
    err = HTM.LoadUI()
    if err < 0:
        print('加载失败, error=' + str(err))
    return err


def load_tool():
    err = HTM.LoadToolUI()
    if err < 0:
        print('加载失败, error=' + str(err))
    return err


def main():
    print('HTM Bsp Version: ', HTM.GetVersionInfo())
    while(True):
        HTM.ShowVersion()
        if bsp_init() < 0:    #初始化
            break
        if load_tool() < 0:    #加载工具面板
            break
        if load_ui() < 0:     #加载面板
            break
        break
    return HTM.Discard()


if __name__ == "__main__":
	sys.exit(int(main() or 0))
	main();
