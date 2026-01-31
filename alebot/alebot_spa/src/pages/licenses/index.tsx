import React, { useEffect, useMemo, useState } from "react";
import { getLicenses } from "@/service/licenses";
import {Alert, Button, IconButton, Stack, Typography} from "@mui/material";
import EditIcon from '@mui/icons-material/Edit';

import styles from "./styles.module.scss";
import LicenseModal from "@/components/LicenseModal/LicenseModal";
import Link from "next/link";

const LicensesPage = () => {
  const [data, setData] = useState([]);
  const [showModal, setSHowModal] = useState(false)
  const isDataExist = useMemo(() => data?.length, [data]);
  const [id,setId] = useState('')
  const [showAlert,setShowAlert] = useState(false)
  useEffect(() => {
    getLicenses().then((data) => {setData(data || [])});
  }, [showAlert]);

  return (
    <div className={`${styles.container} ${!isDataExist ? styles.noData : ""}`}>
      <LicenseModal onPress={setSHowModal} active={showModal} id={id} setShowAlert={setShowAlert}/>
      {showAlert && <Stack sx={{width: '400px', position: 'fixed', right: '20px', bottom: '20px'}} spacing={2}>
        <Alert severity="success">Реферальная ссылка скопирована</Alert>
      </Stack>}
      <div className={styles.content}>
        <div className={styles.tableWrapper}>
          <h2>Доступные лицензии</h2>
          <div className={styles.tableContainer}>
            {isDataExist ? (
              <table>
                <colgroup>
                  <col width="220px" />
                  <col width="210px" />
                  <col width="287px" />
                </colgroup>
                <thead>
                  <tr>
                    <th>
                      <p>
                        <strong>Трейдер</strong> <span>(Тип Лицензии)</span>
                      </p>
                    </th>
                    <th>
                      <p>
                        <strong>Ключ активации</strong>
                      </p>
                    </th>
                    <th>
                      <p>
                        <strong>Номер счета</strong>
                      </p>
                    </th>
                  </tr>
                </thead>
                <tbody>
                  {data.map(
                    ({ id, name, activationKey, tradingAccount }) => {
                      return (
                        <tr key={id}>
                          <td>{name}</td>
                          <td>{activationKey}</td>
                          <td>
                            <Typography sx={{
                              display:'flex',
                              alignItems:'center',
                            }}>
                              {tradingAccount}
                              <IconButton onClick={()=>{
                                setId(id)
                                setSHowModal(true)
                              }}
                                  sx={{
                                backgroundColor:'#2f5548',
                                color:'white',
                                marginLeft:'15px',
                                width:'30px',
                                height:'30px',
                                borderRadius:'7px',
                                '&:hover':{
                                  backgroundColor:'#294b3f',
                                },
                                '& svg':{
                                  fontSize:'15px',
                                  color:'#fff',
                                  fill:'#fff'
                                }
                              }}>
                                      <EditIcon />
                              </IconButton>
                            </Typography>

                          </td>
                        </tr>
                      );
                    }
                  )}
                </tbody>
              </table>
            ) : (
              <div className={styles.noData}>Нет доступных лицензий</div>
            )}
          </div>
        </div>
        <div className={styles.buttonWrapper}>
          <Link href={'/products'}>
            <Button className={styles.buyButton}>
              Купить лицензию на робота
            </Button>
          </Link>
        </div>
      </div>
    </div>
  );
};

export default LicensesPage;
