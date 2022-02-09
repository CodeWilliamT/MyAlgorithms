using namespace std;
#include <iostream>
//巧思 设计题
//分摊大时间操作到小操作上。
//用个备份数组记录取反的数据。取反操作直接在各种修改的时候做在备份数组上。
class Bitset {
    string data, rdata;
    int cnt,size;
    bool flag;
public:
    Bitset(int size) {
        data = string(size,'0');
        rdata= string(size, '1');
        cnt = 0;
        this->size = size;
        flag = 0;
    }

    void fix(int idx) {
        if (!flag&&(data[idx]=='0')) {
            cnt++;
            data[idx] = '1';
            rdata[idx] = '0';
        }
        else if (flag && (rdata[idx] == '0')) {
            cnt++;
            data[idx] = '0';
            rdata[idx] = '1';
        }
    }

    void unfix(int idx) {
        if (!flag && (data[idx] == '1')) {
            cnt--;
            data[idx] = '0';
            rdata[idx] = '1';
        }
        else if (flag && (rdata[idx] == '1')) {
            cnt--;
            data[idx] = '1';
            rdata[idx] = '0';
        }
    }

    void flip() {
        flag = !flag;
        cnt = size - cnt;
    }

    bool all() {
        return cnt == size;
    }

    bool one() {
        return cnt;
    }

    int count() {
        return cnt;
    }

    string toString() {
        return flag?rdata:data;
    }
};
/**
 * Your Bitset object will be instantiated and called as such:
 * Bitset* obj = new Bitset(size);
 * obj->fix(idx);
 * obj->unfix(idx);
 * obj->flip();
 * bool param_4 = obj->all();
 * bool param_5 = obj->one();
 * int param_6 = obj->count();
 * string param_7 = obj->toString();
 */