import { useState } from "react";
import { Form } from "react-bootstrap";
import Button from "react-bootstrap/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import React from "react";

const SearchForm=()=>{
    const[keyword,setKeyword]=useState('');
    const HandleSubmit=(e)=>{
        e.preventDefault();
        window.location.assign(`/blog?k=${String(keyword)}`);
    };
    return (
        <div className="mb-4">
            <Form method="get" onSubmit={HandleSubmit} >
                <Form.Group className="input-group mb-3">
                    <Form.Control
                    type="text"
                    name="k"
                    value={keyword}
                    onChange={(e)=>setKeyword(e.target.value)}
                    aria-label="Enter keyword"
                    aria-describedby="btnSearchPost"
                    placeholder="Enter keyword"/>
                    <Button
                    id="btnSearchPost"
                    variant="outline-secondary"
                    type="submit">
                        <FontAwesomeIcon icon={faSearch}/>

                    </Button>
                </Form.Group>

            </Form>
        </div>
    )
}




export default SearchForm