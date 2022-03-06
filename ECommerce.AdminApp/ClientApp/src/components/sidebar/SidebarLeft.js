import React from "react";
import { Nav } from "react-bootstrap";
import { Link } from "react-router-dom";
import "./sidebarLeft.css"
import { BarChart, Person, Storefront } from '@mui/icons-material';
function SidebarLeft() {
    return (
        <React.Fragment>
            <Nav className="flex-column sidebarLeftWrapper">
                <Nav.Link as={Link} to="/">
                    <BarChart className="featureIcon" />
                    Dashboard
                </Nav.Link>
                <Nav.Link as={Link} to="/users" >
                    <Person className="featureIcon" />
                    User
                </Nav.Link>
                <Nav.Link as={Link} to="/products">
                    <Storefront className="featureIcon" />
                    Products
                </Nav.Link>
            </Nav>
        </React.Fragment>
    );
}

export default SidebarLeft;