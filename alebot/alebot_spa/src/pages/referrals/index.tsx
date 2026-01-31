import * as React from "react";
import {Box, Typography} from "@mui/material";
import {styles} from './styles';
import {useEffect, useState} from "react";
import api from "@/service/api";
import Image from "next/image";
import referralNodata from '../../assets/images/referralNoData.png';
import reload from '../../assets/images/svg/reload.png';
import line from '../../assets/images/svg/line.png';
import ReferralCard from "@/components/ReferralCard/ReferralCard";

export default function StarredPage() {
  const [referrals,setReferrals] = useState([])
  const [total,setTotal] = useState()
  const [active,setActive] = useState()
  const [firstLine,setFirstLine] = useState()
  const [secondLine,setSecondLine] = useState()

    const getData = ()=>{
        api.get('/api/v1/referrals')
            .then(res => {
                setReferrals(res.data.referrals)
                setTotal(res.data.total)
                setActive(res.data.active)
                setFirstLine(res.data.firstLine)
                setSecondLine(res.data.secondLine)
            })
            .catch(e => console.log(e))
    }
    useEffect(()=>{
        getData()
    },[])
  return (
      <Box sx={styles.box}>
          {referrals.length === 0 ?
          <Box>
              <Typography sx={styles.structure}>Структура</Typography>
              <Box sx={styles.noDataWrapper}>
                  <Typography sx={styles.noDataTitle}>Привлекайте партнеров, чтобы видеть данные по
                      команде </Typography>
                  <Image src={referralNodata} style={styles.noDataImage}/>
                  <Box display={'flex'} mt={3} sx={{cursor: 'pointer'}} onClick={getData}>
                      <Image src={reload}/>
                      <Typography sx={styles.reload}>Обновить</Typography>
                  </Box>
              </Box>
          </Box> :
          <Box>
              <Box sx={styles.structureWrapper}>
                  <Image src={line} className='line'/>
                  <Typography sx={styles.structure} style={{margin:'0'}}>Структура</Typography>
                  <Box display={'flex'}>
                      <Box sx={styles.detailsBox}>
                          <span>{total}</span>
                          <p>Всего</p>
                      </Box>
                      <Box sx={styles.detailsBox}>
                          <span>{active}</span>
                          <p>Активных</p>
                      </Box>
                      <Box sx={styles.detailsBox}>
                          <span>{firstLine}</span>
                          <p>Первая линия</p>
                      </Box>
                      <Box sx={styles.detailsBox}>
                          <span>{secondLine}</span>
                          <p>Вторая линия</p>
                      </Box>
                  </Box>
              </Box>
              <Box sx={styles.catalog}>
                  {referrals.map(e => <ReferralCard
                                            active={e.active}
                                            total={e.total}
                                            productCount={e.productCount}
                                            myIncome={e.myIncome}
                                            fullName={e.fullName}
                                            email={e.email}/>)}
              </Box>
              <Box display={'flex'}
                   justifyContent={'center'}
                   my={3}
                   onClick={getData}
                   sx={{cursor: 'pointer'}}>
                  <Image src={reload}/>
                  <Typography sx={styles.reload}>Обновить</Typography>
              </Box>
          </Box>}
      </Box>
  );
}
