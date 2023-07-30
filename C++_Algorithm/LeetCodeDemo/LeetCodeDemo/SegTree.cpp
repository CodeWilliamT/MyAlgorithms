using namespace std;
#include <iostream>
#include <vector>
//�߶�������״���顢���֡�����
//���Ӷ�logN���������Ըġ��顣
//�߶���������1�������������ԣ�2���������������ԣ�3�����Ұ���������ԣ��Դ����ơ�
class SegTree {
private:
    vector<int>T;//�߶�������
    vector<bool> lazy;
    int N;

public:
    SegTree(vector<int>& nums) {
        N = nums.size();
        T = vector<int>(N * 4);
        lazy = vector<bool>(N * 4);
        build(1, 0, N - 1, nums);
    }
    void Update(int ul, int ur, int v = 0) {
        update(1, 0, N - 1, ul, ur, v);
    }
    int Query(int ql, int qr) {
        return query(1, 0, N - 1, ql, qr);
    }
private:
    /// <summary>
    /// �����߶���,������������ֵ(���������С֮���)��
    /// ����������ϣ��ȼ��㹹��������,�����䡣
    /// </summary>
    /// <param name="node">��ǰ������߶�������</param>
    /// <param name="l">Ҫ������ԭ���鵱ǰ��������ԵԪ������</param>
    /// <param name="r">Ҫ������ԭ���鵱ǰ������ұ�ԵԪ������</param>
    void build(int node, int l, int r, vector<int>& nums) {
        if (l == r) {
            //����Ԫ��ʱΪԭ����Ԫ�ر�����������Ҫ��ʲô����
            T[node] = nums[l];
            return;
        }
        int m = (l + r) >> 1;
        build(node << 1, l, m, nums);
        build((node << 1) | 1, m + 1, r, nums);
        pushUp(node, l, r);
    }

    /// <summary>
    /// �������������������Լ�Ҫ��ʲô�������ϼ�������
    /// </summary>
    /// <param name="node">��ǰ������߶�������</param>
    /// <param name="l">Ҫ���µ�ԭ���鵱ǰ��������ԵԪ������</param>
    /// <param name="r">Ҫ���µ�ԭ���鵱ǰ������ұ�ԵԪ������</param>
    void pushUp(int node, int l, int r) {
        if (l == r)
            return;
        int nl = node << 1, nr = nl | 1;
        //�����������������޸�
        T[node] = T[nl] + T[nr];
    }

    /// <summary>
    /// ���ݱ�ǣ������¼�����������
    /// </summary>
    /// <param name="node">��ǰ������߶�������</param>
    /// <param name="l">Ҫ���µ�ԭ���鵱ǰ��������ԵԪ������</param>
    /// <param name="r">Ҫ���µ�ԭ���鵱ǰ������ұ�ԵԪ������</param>
    void pushDown(int node, int l, int r) {
        if (l == r || !lazy[node])
            return;
        int nl = node << 1, nr = nl | 1;
        int m = (l + r) / 2;
        lazy[nl] = !lazy[nl];
        lazy[nr] = !lazy[nr];
        //�����������������޸�
        T[nl] = (m - l + 1) - T[nl];
        T[nr] = (r - (m + 1) + 1) - T[nr];
        lazy[node] = 0;
    }
    /// <summary>
    /// �޸�ԭ����Ԫ�أ�Ȼ������������ݡ�
    /// �޸��������������ı��������Էǵ�Ԫ����������
    /// ֱ����ѯ���޸ĵ�Ҫ�޸�Ԫ�ص�Ԫ���ˣ��޸��������޸��������ݡ�
    /// </summary>
    /// <param name="node">��ǰ������߶�������,��1��ʼ</param>
    /// <param name="l">�߶����ڵ�����ָ���ԭ������������ԵԪ������</param>
    /// <param name="r">�߶����ڵ�����ָ���ԭ����������ұ�ԵԪ������</param>
    /// <param name="ul">ԭ������޸�������߽�����(����)</param>
    /// <param name="ur">ԭ������޸������ұ߽�����(����)</param>
    /// <param name="v">ԭ����Ҫ�ĳɵ�ֵ,����ȡ������û�õ�</param>
    void update(int node, int l, int r, int ul, int ur, int v = 0) {
        if (ul <= l && r <= ur) {
            //��ǰ����Ϊ���޸������Ӽ���ֱ��ͨ���仯���޸ĵ�ǰ����,���ǵ�Ԫ��,���������ǡ�
            T[node] = (r - l + 1) - T[node];
            lazy[node] = !lazy[node];
            return;
        }
        pushDown(node, l, r);
        int m = (l + r) / 2;
        if (ul <= m && l <= ur) update(node << 1, l, m, ul, ur, v);
        if (ul <= r && m < ur) update(node << 1 | 1, m + 1, r, ul, ur, v);
        pushUp(node, l, r);
    }


    /// <summary>
    /// ��ѯԭ�����ĳ���������
    /// </summary>
    /// <param name="node">��ǰ������߶�������</param>
    /// <param name="l">��ǰ������߶�������ָ���ԭ������������ԵԪ������</param>
    /// <param name="r">��ǰ������߶�������ָ���ԭ����������ұ�ԵԪ������</param>
    /// <param name="ql">Ҫ��ѯ��������߽�����()</param>
    /// <param name="qr">Ҫ��ѯ������������ұ߽�����</param>
    /// <returns>�������ĳ����</returns>
    int query(int node, int l, int r, int ql, int qr) {
        if (l >= ql && r <= qr) {
            return T[node];
        }
        if (r < ql || l > qr) {
            return 0;
        }
        pushDown(node, l, r);
        int rst = 0;
        int m = (l + r) >> 1;
        if (ql <= m && l <= qr)
            rst += query(node << 1, l, m, ql, qr);
        if (ql <= r && m < qr)
            rst += query(node << 1 | 1, m + 1, r, ql, qr);
        return rst;
    }
};