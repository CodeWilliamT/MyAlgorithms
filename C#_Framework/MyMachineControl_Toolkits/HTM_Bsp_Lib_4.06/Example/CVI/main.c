#include <userint.h>
#include <utility.h>
#include <ansi_c.h>
#include "Htm_Bsp_Lib.h"


int main (int argc, char *argv[])
{

	HTM_INIT_PARA _init = {0};
	char _path[260] = {0};
	GetProjectDir(_path);
	sprintf(_init.para_file, "%s\\htm_paras.db", _path);
	_init.max_axis_num = 10;		 //����Ŀ
	_init.max_io_num = 30;			 //IO��Ŀ
	_init.max_dev_num = 6;			 //�����豸���������塢��Դ�ȣ�
	_init.use_aps_card = 1;
	_init.use_htnet_card = 1;
	
	_init.offline_mode = 1;
	
	int err = 0;
	if((err = HTM_Bsp_Init(&_init))<0)
	{
		sprintf(_path, "Erorr code: %d", err);
		MessagePopup("error", _path);
		return err;
	}
	HTM_Bsp_LoadUI();
	return HTM_Bsp_Discard();
}

