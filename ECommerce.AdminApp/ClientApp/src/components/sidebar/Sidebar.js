import React from 'react'
import { Routes, Route } from "react-router-dom"
import { Container, Row, Col } from "react-bootstrap"
import SidebarLeft from './SidebarLeft'
import Dashboard from '../pages/dashboard/Dashboard'
import User from "../pages/user/User"
import "./sidebar.css"

function Sidebar() {
    return (
        <Container fluid className='sidebar'>
            <Row>
                <Col sm={2} className="sidebarWrapper">
                    <SidebarLeft />
                </Col>
                <Col sm={10} className="sidebarContent">
                    <Routes>
                        <Route path="/" element={<Dashboard />} />
                        <Route path="/users" element={<User />} />
                    </Routes>
                </Col>
            </Row>
        </Container>
    )
}

export default Sidebar