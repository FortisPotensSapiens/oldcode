import { Button, Grid, styled, Typography } from '@mui/material';

const StyledGrid = styled(Grid)({
  height: '100%',
  textAlign: 'center',
});

const NoOrders = ({ handleOpenDialog }: { handleOpenDialog: () => void }) => {
  return (
    <>
      <StyledGrid alignContent="center" container flexDirection="column" justifyContent="center">
        <Grid item paddingBottom={2}>
          <Typography variant="body1">Сделайте ваш первый заказ</Typography>
        </Grid>
        <Grid item>
          <Button onClick={handleOpenDialog} variant="contained">
            Создать индивидуальный заказ
          </Button>
        </Grid>
      </StyledGrid>
    </>
  );
};

export default NoOrders;
