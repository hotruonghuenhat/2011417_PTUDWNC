import React ,{useEffect} from "react";

const About=()=>{
    useEffect(()=>{
        document.title="About"
    },[]);
    return (
        <div>
            <h1 className="text-center">Trường đại học Đà Lạt đẹp nhất Việt Nam</h1>
            
            <iframe className='w-100' width="1920" height="1080" src="https://www.youtube.com/embed/lH2tAdLt5NU" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe>        
        </div>       
    )
}
export default About