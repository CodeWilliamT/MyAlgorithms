# -*-coding:utf-8 -*-
import baostock as bs
import os
import pandas as pd
from pathlib import Path

#判定低开
def IsDikai(df,idx):
    preclose=float(df.at[idx,'preclose'])
    open=float(df.at[idx,'open'])
    return preclose >= open

#判绿，判定纺锤线或绿色实体
def IsLv(df,idx):
    pctChg=float(df.at[idx,'pctChg'])
    return pctChg < 2

#判定纺锤线
def Isfangchuixian(df,idx):
    pctChg=float(df.at[idx,'pctChg'])
    return abs(pctChg) < 2

#判定长下影线
def IsChangxiayingxian(df,idx):
    open=float(df.at[idx,'open'])
    close=float(df.at[idx,'close'])
    low=float(df.at[idx,'low'])
    high=float(df.at[idx,'high'])
    solid=abs(open-close)
    lowline=min(open,close)-low
    highline=high-max(open,close)
    #剔除无或过短下影线
    if lowline/close<0.01:
        return False
    #剔除上影线高度超过下影线
    if highline/lowline>1:
        return False
    size=solid/lowline
    #剔除实体高度超过下影线
    if size >1:
        return False
    return True

#判定伞形线
def IsSanxingxian(df,idx):
    open=float(df.at[idx,'open'])
    close=float(df.at[idx,'close'])
    low=float(df.at[idx,'low'])
    high=float(df.at[idx,'high'])
    preclose=float(df.at[idx,'preclose'])
    #剔除股价收盘上升大于2%的
    if close/preclose-1>0.02:
        return False
    lowline=min(open,close)-low
    #剔除无或过短下影线
    if lowline/close<0.01:
        return False
    highline=high-max(open,close)
    #剔除上影线高度超过下影线1/2
    if highline/lowline>0.5:
        return False
    solid=abs(open-close)
    size=solid/lowline
    #剔除实体高度超过下影线1/2
    if size >0.5:
        return False
    return True

#判定是否可选，未停牌，盈利净资产不为负
def IsKexuan(df,idx):
    if result.at[0,'turn']=='':
        return False
    if float(result.at[0,'peTTM'])<=0:
        return False
    if float(result.at[0,'pbMRQ'])<=0:
        return False
    return True

#判定是否绩优，未停牌，TTM<150，净资产收益率大于10%，
def IsJiyou(df,idx):
    if result.at[0,'turn']=='':
        return False
    peTTM=float(result.at[0,'peTTM'])
    pbMRQ=float(result.at[0,'pbMRQ'])
    roi=pbMRQ/peTTM
    if peTTM<=0:
        return False
    if pbMRQ<=0:
        return False
    if peTTM>150:
        return False
    if roi<=0.1:
        return False
    return True

#读取code集,如不存在则获取行业code集
def get_codeSet(filename="C:/Stock Result/DataSet.csv"):
    file=Path(filename)
    if file.exists():
        data_df.read_csv(filename, encoding="utf_8_sig", index=False)
        return data_df
    return bs.query_stock_industry().get_data()

#获取主A股代码集合
def get_dateAMainCode(date):
    rs = bs.query_all_stock(date)
    df = pd.DataFrame()
    result=rs.get_data()
    df=df.append(result.loc[result['code'].str.contains('sz.00')])
    df=df.append(result.loc[result['code'].str.contains('sh.60')])
    return df

#获取指定code集的某日K线数据
def get_dfDateData(stock_df,date):
    filename="C:/Stock Result/+"+date+"Data.csv"
    file=Path(filename)
    if file.exists():
        data_df.read_csv(filename, encoding="utf_8_sig", index=False)
        return data_df
    data_df = pd.DataFrame()
    for code in stock_df["code"]:
        print("Downloading :" + code)
        k_rs = bs.query_history_k_data_plus(code, "date,code,peTTM,pbMRQ,pctChg,open,high,low,close,preclose,turn", date, date,frequency="d", adjustflag="3")
        data_df = data_df.append(k_rs.get_data())
    data_df.to_csv(filename, encoding="utf_8_sig", index=False)
    return data_df

#### 登陆系统 ####
lg = bs.login()
# 显示登陆返回信息
print('login respond error_code:'+lg.error_code)
print('login respond  error_msg:'+lg.error_msg)
dateS="2022-02-24"
dateE="2022-02-27"
outputPath="C:/Stock_Result/"
if os.path.isdir(outputPath)==False:
    os.mkdir(outputPath)
#### 获取某一天的全市场的证券和指数代码 ####
code_df=get_codeSet()
df = pd.DataFrame()
#### 获取沪深A股历史K线数据 ####
for code in code_df['code']:
    # 详细指标参数，参见“历史行情指标参数”章节
    rs = bs.query_history_k_data(code,
        "date,code,peTTM,pbMRQ,pctChg,open,high,low,close,preclose,turn",
        dateS, dateE,
        frequency="d", adjustflag="3")
    if rs.error_code == '0' :
        result = rs.get_data()
        n = result.shape[0]
        if n <= 0:
            continue
        #判定可选
        if IsKexuan(result,0)==False:
            continue
        #判定市盈率低于50
        if float(result.at[0,'peTTM'])>50:
            continue
        #判绿
        if IsLv(result,0)==False:
            continue
        #判定伞形线
        if IsSanxingxian(result,1):
            df=df.append(result.loc[1])

                
#### 结果集输出到csv文件 ####

df=pd.merge(code_df,df)
df['peTTM'] = df['peTTM'].astype(float)
df['pbMRQ'] = df['pbMRQ'].astype(float)
df['close'] = df['close'].astype(float)
#以peTTM进行升序排序
df['净资产收益率']=df['pbMRQ']/df['peTTM']
df['高估指数']=df['peTTM']+1/df['净资产收益率']
df_sort = df.sort_values(by=['净资产收益率'],ascending=False)
targetColumns=['updateDate','industry','industryClassification','code','code_name','peTTM','高估指数','净资产收益率','pbMRQ','pctChg','close','open','preclose','high','low',"turn",'date']
print(df_sort)
df_sort.to_csv(outputPath+dateE+"MorningStar.csv",encoding='utf_8_sig', index=False,columns=targetColumns)
print("success")
