import axiosClient from './axiosClient'

const blogApi = {
  getAll(params) {
    const url = '/posts'
    return axiosClient.get(url, { params })
  }
}

export default blogApi
