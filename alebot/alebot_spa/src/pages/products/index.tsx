import * as React from "react";
import Container from "@mui/material/Container";
import { useEffect, useState } from "react";
import api from "@/service/api";
import ProductCard from "@/components/ProductCard/ProductCard";
import { styles } from './style';
import SectionTitle from "@/components/SectionTitle/SectionTitle";
import {Box} from "@mui/material";
import LicenseModal from "@/components/LicenseModal/LicenseModal";

export default function TasksPage() {
  const [products, setProducts] = useState([])
  const [buyedProducts, setBuyedProducts] = useState([])
  const [showModal, setShowModal] = useState(false)
  const [activeId, setActiveId] = useState('')
  const [showAlert,setShowAlert] = useState(false)
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
  const handleModalOpen = (id) => {
    setActiveId(id)
    setShowModal(true)
    handleTransaction(id)
  }
  useEffect(() => {
    api.get('/api/v1/shop/products/with-licenses')
      .then(res => {
          setProducts(res.data.merch)
          setBuyedProducts(res.data.buyedLicenses)
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
          {buyedProducts.length !== 0 && <SectionTitle style={{fontSize: '20px'}}>Купленные лицензии</SectionTitle>}
          {buyedProducts.length !== 0 && <Container sx={{...styles.container,marginBottom:'48px'}}>
          {buyedProducts.map(e => <ProductCard key={e.merchId}
                                                   id={e.merchId}
                                                   name={e.licenseName}
                                                   handleModalOpen={handleModalOpen}
                                                   shortDescription={e.merchDescription}
                                                   licenseKey={e.licenseKey}
                                                   tradingAccountNumber={e.tradingAccountNumber}
                                                   photo={e.photo}
                                                   isBuyed={true}
                                                   userLicenseId={e.userLicenseId}/>)}
          </Container>}
          {products.length !== 0 && <SectionTitle style={{fontSize: '20px'}}>Доступные лицензии</SectionTitle>}
          {products.length !== 0 && <Container sx={styles.container}>
          {products.map(e => <ProductCard key={e.merchId}
                                              id={e.merchId}
                                              name={e.name}
                                              price={e.price}
                                              handleModalOpen={handleModalOpen}
                                              shortDescription={e.description}
                                              photo={e.photo}/>)}
          </Container>}
      </Box>
  );
}
