import React, { useEffect, useState } from 'react';
import { useRouter } from "next/router";
import api from "@/service/api";
import { Box, Typography } from "@mui/material";
import { styles } from './style';
import Image from "next/image";
import Button from "@mui/material/Button";
import { isImageFileExtensionValid } from "@/helpers/imageValidator";
import productImage from "@/assets/images/productimage.jpeg";
import SectionTitle from "@/components/SectionTitle/SectionTitle";
import ProductModal from "@/components/ProductModal/ProductModal";
import MuiMarkdown from 'mui-markdown';
const Index = () => {
    const [showModal, setShowModal] = useState(false)
    const [data, setData] = useState({
        fullDescription: '',
        name: '',
        photo: '',
        price: 0,
    })
    const router = useRouter()
    useEffect(() => {
        if (router?.query?.id)
            api.get(`/api/v1/shop/products/${router.query.id}`)
                .then(res => setData(res.data))
                .catch(e => console.log(e))
    }, [router])
    const handleTransaction = (id) => {
        let body = {
            merchId: id,
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
    return (
        <Box width={'100%'}>
            <ProductModal onPress={setShowModal} active={showModal} id={router.query.id?.toString()} />
            <SectionTitle>{data.name}</SectionTitle>
            <Box sx={styles.box}>
                <Box sx={styles.imageWrapper}>
                    <Image src={isImageFileExtensionValid(data.photo) ? `data:image/png;base64,${data.photo}` : productImage}
                        alt={data.name}
                        style={styles.photo}
                        width={250}
                        height={250}
                    />
                    <Typography sx={styles.title}>{data.name}</Typography>
                    <Box sx={{
                        '@media only screen and (max-width: 640px)': {
                            display:'none',
                        },
                    }}>
                        <Typography sx={styles.price}>{data.price} USDT</Typography>
                        {router.query.id ? <Button sx={styles.button} onClick={() => handleTransaction(router.query.id)}>Купить</Button> : <div></div>}
                    </Box>
                </Box>
                <Box>
                    <Typography sx={styles.des}>Описание</Typography>
                    <br />
                    <MuiMarkdown>
                        {data.fullDescription}
                    </MuiMarkdown>
                    <Box sx={{
                        display:'none',
                        '@media only screen and (max-width: 640px)': {
                            display: 'flex',
                            flexDirection:'column',
                            alignItems:'center',
                            marginBottom:'50px',
                        },
                    }}>
                        {router.query.id ? <Button sx={styles.button} onClick={() => handleTransaction(router.query.id)}>Купить</Button> : <div></div>}
                        <Typography sx={styles.price}>{data.price} USDT</Typography>
                    </Box>
                </Box>
            </Box>
        </Box>

    );
};

export default Index;