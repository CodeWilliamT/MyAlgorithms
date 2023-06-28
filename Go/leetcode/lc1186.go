package lc1186

func maximumSum(arr []int) int {
	n := len(arr)
	if n == 1 {
		return arr[0]
	}
	subHeadMax := make([]int, n)
	subTailMax := make([]int, n)
	subHeadMax[0] = arr[0]
	subTailMax[n-1] = arr[n-1]
	for i := 1; i < n-1; i++ {
		subHeadMax[i] = arr[i] + max(0, subHeadMax[i-1])
		subTailMax[n-1-i] = arr[n-1-i] + max(0, subTailMax[n-1-(i-1)])
	}
	rst := max(max(0, arr[0])+subTailMax[1], max(0, arr[n-1])+subHeadMax[n-2])
	var tmpmax int
	for i := 1; i < n-1; i++ {
		tmpmax = max(0, arr[i]) + max(max(subHeadMax[i-1], subTailMax[i+1]), subHeadMax[i-1]+subTailMax[i+1])
		rst = max(rst, tmpmax)
	}
	return rst
}

func max(a, b int) int {
	if b > a {
		return b
	}
	return a
}
