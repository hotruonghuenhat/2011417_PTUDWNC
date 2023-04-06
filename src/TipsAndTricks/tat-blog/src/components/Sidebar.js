import React from 'react'
import PropTypes from 'prop-types'
import SearchForm from './SearchForm'
import CategoriesWidget from './CategoriesWidget'

Sidebar.propTypes = {}

function Sidebar(props) {
  return (
    <div className='pt-4 ps-2'>
      <SearchForm />
      <CategoriesWidget />
      <h1>Tìm kiếm bài viết</h1>
      <h1>Các chủ đề</h1>
      <h1>Bài viết nổi bật</h1>
      <h1>Đăng ký nhận tin mới</h1>
      <h1>Bài viết nổi bật</h1>
      <h1>Đăng ký nhận tin mới</h1>
      <h1>Tag Cloud</h1>
    </div>
  )
}

export default Sidebar
