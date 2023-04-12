import React from "react";
import SearchForm from "./SearchForm";
import CategorieWidget from "./CategoriesWidget";

const Sidebar=()=>{
    return (
        <div className="pt-4 ps-2">
           <SearchForm/>
            <CategorieWidget/>
            <h1>Bài viết nổi bật</h1>
            <h1>Đăng kí nhận tin mới</h1>
            <h1>Tag Cloud</h1>
        </div>
    )
}




export default Sidebar;