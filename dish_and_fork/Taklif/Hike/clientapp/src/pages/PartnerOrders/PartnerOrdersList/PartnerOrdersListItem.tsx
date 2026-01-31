import { Box, Grid, Typography } from '@mui/material';
import ListItem from '@mui/material/ListItem';
import styled from '@mui/system/styled';
import { Button } from 'antd';
import { SyntheticEvent } from 'react';

import { getStyledStatus, Row } from '../utils';

const StyledItem = styled(ListItem)({
  cursor: 'pointer',
});

const PartnerOrdersListItem = ({
  onRowClick,
  onReadyToDeliverClick,
  row,
}: {
  row: Row;
  onRowClick: (orderId: string) => void;
  onReadyToDeliverClick: (orderId: string) => void;
}): JSX.Element => {
  const onRowClickCallback = () => {
    onRowClick(row.id);
  };

  const onReadyToDeliverClickCallback = (event: SyntheticEvent) => {
    event.stopPropagation();

    onReadyToDeliverClick(row.id);
  };

  const onClickTargetCallback = (event: SyntheticEvent) => {
    event.stopPropagation();

    window.open(row.sellerDeliveryTrackingUrl, '_blank')?.focus();
  };

  return (
    <StyledItem onClick={onRowClickCallback}>
      <Grid container justifyContent="space-between">
        <Grid item>
          <Box>
            <Typography variant="h6">Заказ № {row.number}</Typography>
            <Typography variant="caption">{row.createDate}</Typography>
          </Box>
          {row.isReadyToDelivery ? <Button onClick={onReadyToDeliverClickCallback}>Отправить заказ</Button> : undefined}

          {row.sellerDeliveryTrackingUrl ? (
            <Button onClick={onClickTargetCallback}>Отследить доставку</Button>
          ) : undefined}
        </Grid>
        <Grid item>
          <Box textAlign="center">
            <Typography variant="body1">{row.buyerName}</Typography>
          </Box>
          <Box textAlign="center">
            <Typography variant="body1">{getStyledStatus(row.status)}</Typography>
          </Box>
        </Grid>
      </Grid>
    </StyledItem>
  );
};

export default PartnerOrdersListItem;
