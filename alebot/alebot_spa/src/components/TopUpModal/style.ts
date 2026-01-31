export const styles = {
    box: {
        width: '100vw',
        height: '100vh',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        position: 'fixed',
        right: '-100%',
        opacity: 0,
        top: '0',
        backgroundColor: '#1E1E1E85',
        zIndex: 99999,
        transition: '0.7s',
        '&.active': {
            right: '0',
            opacity: 1,
        },
        '@media only screen and (max-width: 500px)': {
            padding:'0 20px'
        }
    },
    boxModalMui:{
        justifyContent: 'center',
        alignItems: 'center',
        right: '0',
        zIndex: 9999999999,
        position: 'absolute' as 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        width: 'fit-content',
        '@media only screen and (max-width: 500px)': {
            width:'calc(100% - 32px)',
        }
    },
    wrapper: {
        width: '465px',
        borderRadius: '50px',
        background: '#FFF',
        padding: '78px 48px',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        position: 'relative',
        height: 'fit-content',
        '@media only screen and (max-width: 500px)': {
            width:'calc(100%)',
        }

    },
    title: {
        color: '#1E1E1E',
        fontSize: '20px',
        fontWeight: '700',
        lineHeight: 'normal',
        textAlign: 'center',
    },
    text: {
        color: '#1E1E1E',
        textAlign: 'center',
        fontSize: '12px',
        fontWeight: '400',
        lineHeight: 'normal',
        marginTop: '21px',
    },
    inputWrapper: {
        width: '100%',
        marginTop: '40px',
        height: '66px',
        borderRadius: '6px',
        background: '#FFF',
        border: '1px solid #F5F5F5',
        padding: '18px',
        paddingLeft:'14px',
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
    },
    summery: {
        color: '#1E1E1E',
        fontSize: '10px',
        fontWeight: '700',
        lineHeight: 'normal',
        marginBottom: '8px'
    },
    maxPrice: {
        color: '#1E1E1E',
        fontSize: '10px',
        fontWeight: '400',
        lineHeight: 'normal',
        textAlign: 'right',
        marginBottom: '2px',
        '& b': {
            color: '#2F5549',
            fontSize: '10px',
            fontWeight: '700',
            lineHeight: 'normal',
        }
    },
    cardInput: {
        width: '100%',
        height: '40px',
        border: '1px solid #F5F5F5',
        marginTop: "10px",
        textIndent: '18px',
    },
    cancelButton: {
        width: '174px',
        height: '35px',
        padding: '15px 39px',
        justifyContent: 'center',
        alignItems: 'center',
        borderRadius: '5px',
        background: '#BFBFBF',
        color: '#FFF',
        fontSize: '14px',
        fontWeight: '500',
        lineHeight: 'normal',
        '&:hover': {
            background: '#BFBFBF',
            color: '#FFF',
        }
    },
    continueButton: {
        width: '174px',
        height: '35px',
        padding: '15px 39px',
        justifyContent: 'center',
        alignItems: 'center',
        borderRadius: '5px',
        background: '#2F5549',
        color: '#FFF',
        fontSize: '14px',
        fontWeight: '500',
        lineHeight: 'normal',
        '&:hover': {
            background: '#2F5549',
            color: '#FFF',
        }
    },
    closeButton: {
        position: 'absolute',
        top: '37px',
        right: '44px',
        cursor: 'pointer',
    },
    moneyInput: {
        border: 'none !important',
        outline: 'none !important',
        marginLeft: '2px',
        fontSize: '16px',
        width: '100%',
    },
    invalidInput: {
        borderColor: 'red',
        borderStyle: 'solid',
        marginLeft: '2px',
        fontSize: '16px',
        width: '100%',
    }
}