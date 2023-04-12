import React ,{useEffect} from "react";

const Contact=()=>{
    useEffect(()=>{
        document.title="Contact"
    },[]);
    return (
        <div>
            <h1 className="text-center">Liên hệ Trường đại học Đà Lạt</h1>           
        </div>       
    )
}

export default Contact