import React from "react";
import PropTypes from "prop-types";
import { Outlet } from "react-router-dom";

Layout.propTypes = {};

function Layout(props) {
  return (
    <>
      <Outlet />
    </>
  );
}

export default Layout;
