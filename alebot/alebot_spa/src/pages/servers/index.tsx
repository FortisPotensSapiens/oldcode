import * as React from 'react';
import {useEffect, useState} from "react";
import {Box, Button} from "@mui/material";
import SectionTitle from "@/components/SectionTitle/SectionTitle";
import Container from "@mui/material/Container";
import ProductCard from "@/components/ProductCard/ProductCard";
import api from "@/service/api";
import {styles} from './style'
import ServerCard from "@/components/ServerCard/ServerCard";

export default function StarredPage() {
    const [merch, setMerch] = useState([])
    const [buyedServers, setBuyedServers] = useState([])
    const [activeId, setActiveId] = useState('')
    const handleTransaction = (id, userServerId) => {
        let body = {
            merchId: id,
            userServerId: userServerId
        }
        api.post(`/api/v1/shop/orders`, body)
            .then(res => {
                if (res.status === 200) {
                    api.post(`/api/v1/shop/cryptocloud/invoicies?orderId=${res.data}`)
                        .then(res => {
                            window.location.href = res.data.pay_url;
                        })
                        .catch(e => console.log(e))
                }
            })
            .catch(e => console.log(e))
    }
    const handleModalOpen = (id, userServerId) => {
        setActiveId(id)
        handleTransaction(id, userServerId)
    }
    useEffect(() => {
        api.get('/api/v1/shop/products/servers-vds')
            .then(res => {
                setMerch(res.data.merch)
                setBuyedServers(res.data.buyedSevers)
            })
            .catch(e => console.log(e))
    }, [])
    return (
        <Box sx={{
            backgroundColor:'#fff',
            borderRadius:'20px',
            padding:'24px',
            width:'100%',
        }}>
            {buyedServers.length !== 0 && <SectionTitle style={{fontSize: '20px'}}>Купленные сервера VDS</SectionTitle>}
            {buyedServers.length !== 0 && <Container sx={{...styles.container,marginBottom:'48px'}}>
                {buyedServers.map(e => <ServerCard key={e.merchId}
                                                   id={e.merchId}
                                                   name={e.serverName}
                                                   shortDescription={e.merchDescription}
                                                   login={e.login}
                                                   password={e.password}
                                                   price={e.merchPrice}
                                                   created={e.created}
                                                   expirationDate={e.expirationDate}
                                                   serverAddress={e.serverAddress}
                                                   handleModalOpen={handleModalOpen}
                                                   photo={e.photo}
                                                   userServerId={e.userServerId}
                />)}
            </Container>}
            {merch.length !== 0 && <SectionTitle style={{fontSize: '20px'}}>Доступные сервера</SectionTitle>}
            {merch.length !== 0 && <Container sx={styles.container}>
                {merch.map(e => <ServerCard key={e.merchId}
                                            id={e.merchId}
                                            name={e.name}
                                            price={e.price}
                                            color={'green'}
                                            handleModalOpen={handleModalOpen}
                                            shortDescription={e.description}
                                            photo={e.photo}/>)}
            </Container>}
        </Box>
    )
}
