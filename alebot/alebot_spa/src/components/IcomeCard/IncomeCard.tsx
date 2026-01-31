import React from 'react';
import {Accordion, AccordionSummary, Box, Typography} from "@mui/material";
import AccordionDetails from '@mui/material/AccordionDetails';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import informationcircle from '../../assets/images/svg/balance/informationcircle.png'
import {styles} from './style';
import Image from "next/image";
type Props = {
    income:{
        total:number,
        products:number,
    }
}
const IncomeCard:React.FC<Props> = ({income}) => {
    return (
        <Box sx={styles.box}>
            <Accordion sx={{background:'#404040'}} defaultExpanded = {true}>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon />}
                    aria-controls="panel1-content"
                    id="panel1-header"
                >
                    <Typography sx={styles.sectionTitle}>Мой доход</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <Typography sx={styles.infoText}>Всего: <b>$ {income?.total}</b> <span></span> Продукты: <b>$ {income?.products}</b></Typography>
                    <Box sx={{marginTop:'25px',display:'flex',paddingBottom:'15px'}}>
                        <Typography sx={styles.iText}>Расчет спреда в разработке</Typography>
                        <Box sx={styles.iIcon}>
                            <Image src={informationcircle} />
                            <Box className='popOver'>
                                Спред начисляется. Расчет и вывод находятся в разработке
                            </Box>
                        </Box>
                    </Box>
                </AccordionDetails>
            </Accordion>
        </Box>
    );
};

export default IncomeCard;