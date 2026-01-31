export const styles = {
    box:{
        display:'flex',
        width:'100%',
        minHeight:'100px',
        borderRadius: '20px',
        background: '#FFF',
        padding:'22px 37px',
        flexDirection:'column',
    },
    sectionTitle:{
        color: '#404040',
        fontSize: '20px',
        fontWeight: '600',
    },
    sliderSection:{
        position:'relative',
        marginTop:'23px',
        '& .slick-arrow':{
            position: 'absolute',
            right:'5px !important',
            left:'unset',
            top:'-35px',
            bottom:'unset',
            borderRadius:'100%',
            overflow:'hidden',
            '&:before':{
                background:'#000',
                fontSize:'25px',
                margin:'-5px',
            }
        },
        '& .slick-prev':{
            right:'35px !important',
        }
    },
    card:{
        display: 'flex',
        borderRadius: '7px',
        border: '0.5px solid #D9D9D9',
        background: '#FFF',
        boxShadow: '-2px 2px 7px 0px rgba(0, 0, 0, 0.04)',
        height:'115px',
        marginRight:'5px',
        '& img':{
            width: '85px',
            height: '115px',
            borderRadius: '7px',
        }
    },
    cardTitle:{
        color: '#1E1E1E',
        fontSize: '12px',
        fontWeight: '700',
        lineHeight: 'normal',
    },
    cardButton:{
        borderRadius: '2px',
        background: '#2F5549',
        display: 'flex',
        width: '106px',
        height: '28px',
        padding: '15px 39px',
        justifyContent: 'center',
        alignItems: 'center',
        color: '#FFF',
        textAlign: 'center',
        fontSize: '8px',
        fontWeight: '700',
        '&.continues': {
            background: '#be8b20',
        },
        '&.start': {
            background: '#2f5549',
        }
    },
    cardInfo:{
        display:'flex',
        flexDirection: 'column',
        justifyContent:'space-between',
        padding: '10px',
    }
}