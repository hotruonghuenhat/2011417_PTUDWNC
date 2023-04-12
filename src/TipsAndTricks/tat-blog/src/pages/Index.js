import React,{useEffect,useState} from "react";
import PostItem from "../Components/PostItem";
import { getPosts } from "../Services/BlogRespository";
import Pager from "../Components/Pager";
import { useLocation } from "react-router-dom";

const Index=()=>{
    const [postList,setPostList]=useState([]);
    const [metadata,setMetadata]=useState({});

    function useQuery(){
        const {search}=useLocation();
        return React.useMemo(()=>new URLSearchParams(search),[search]);
    }
    let query=useQuery(),
        k=query.get('k')?? '',
        p=query.get('p')?? 1,
        ps=query.get('ps') ?? 10;

    useEffect(() => {
        document.title="Trang chủ";
        getPosts(k , Number(ps), Number(p)).then(data=>{
            if(data){
                setPostList(data.items) 
                setMetadata(data.metadata)
            }else{
                setPostList([])
            }
        })
    },[k,p,ps])
    useEffect(()=>{
        window.scrollTo(0,0);
    },[postList])
    if(postList.length>0){
        return(
            <div className="p-4">
                {postList.map((item,index)=>{
                    return(
                        <PostItem key={index} postItem={item}/>
                    )
                })}
                <Pager postquery={{'keyword':k}} metadata={metadata}/>
            </div>
        );
    }else{
        return (<></>)
    }
}
export default Index;