import ChatIcon from '@mui/icons-material/Chat';
import { Box, Button, Grid, Paper, styled } from '@mui/material';
import { Typography } from 'antd';
import dayjs from 'dayjs';
import LocalizedFormat from 'dayjs/plugin/localizedFormat';
import timezone from 'dayjs/plugin/timezone';
import utc from 'dayjs/plugin/utc';
import { SyntheticEvent } from 'react';

import { FileReadModel, MerchandiseUnitType, OfferReadModel } from '~/types';
import { getCurrencySymbol, getNormalizedServingSize } from '~/utils';
import { Images } from '~/views/PartnerProductView/Images';

import { StyledHeader, StyledShowMoreTextTextField } from './styled';

dayjs.extend(LocalizedFormat);
dayjs.extend(utc);
dayjs.extend(timezone);

const StyledPaperContainer = styled(Paper)(({ theme }) => ({
  cursor: 'pointer',
  '&:hover': {
    boxShadow: theme.shadows[2],
  },
}));

const IndividualOrderResponsesItem = ({
  onChatOpen,
  response,
  onClick,
  onOrder,
  showSelectSeller = false,
  showChat = false,
}: {
  response: OfferReadModel;
  showSelectSeller: boolean;
  showChat?: boolean;
  onChatOpen: (sellerId: string) => void;
  onOrder: (offerId: string) => void;
  onClick: (offerId: string) => void;
}) => {
  const onChatOpenCallback = (e: SyntheticEvent) => {
    e.stopPropagation();

    onChatOpen(response.id);
  };

  const onClickCallback = () => {
    onClick(response.id);
  };

  const onOrderCallback = (e: SyntheticEvent) => {
    e.stopPropagation();

    onOrder(response.id);
  };

  return (
    <StyledPaperContainer onClick={onClickCallback}>
      <Box padding={2}>
        <Grid container>
          <Grid item xs={11}>
            <StyledHeader color="primary.main" variant="h6">
              <Box>{response.seller.title}</Box>
              {showChat ? (
                <Box
                  onClick={onChatOpenCallback}
                  paddingLeft={1}
                  style={{
                    cursor: 'pointer',
                  }}
                >
                  <ChatIcon />
                </Box>
              ) : undefined}
            </StyledHeader>
          </Grid>
          <Grid item sm={1} xs={12}>
            <Box color="text.lightGrey">#{response.number}</Box>
          </Grid>
          <Grid item sm={3}>
            <Grid container spacing={1}>
              <Grid item>Цена:</Grid>
              <Grid item>
                <strong>
                  {response.sum} {getCurrencySymbol('Rub')}
                </strong>
              </Grid>
            </Grid>
          </Grid>
          <Grid item sm={8} xs={12}>
            Дата готовности до: <strong>{dayjs(response.date).local().format('L')}</strong>
          </Grid>
          <Grid item sm={12} xs={12}>
            Вес порции брутто:{' '}
            <strong>
              {getNormalizedServingSize(response.servingGrossWeightInKilograms, MerchandiseUnitType.Kilograms)}
            </strong>
          </Grid>
          <Grid item paddingTop={2} xs={12}>
            <StyledShowMoreTextTextField>{response.description}</StyledShowMoreTextTextField>
          </Grid>

          {response?.images ? (
            <Grid item paddingTop={2} xs={12}>
              <Typography.Text>Примеры работ: </Typography.Text>
              <Images
                addabled={false}
                isDraggingEnabled={false}
                max={999}
                showCrow={false}
                cropping={false}
                value={response.images as FileReadModel[]}
                containerProps={{
                  display: 'flex',
                  overflow: 'scroll',
                }}
                itemProps={{
                  paddingRight: 2,
                }}
              />
            </Grid>
          ) : undefined}

          {showSelectSeller ? (
            <Grid item paddingTop={2}>
              <Button onClick={onOrderCallback} variant="outlined">
                Указать исполнителем
              </Button>
            </Grid>
          ) : undefined}
        </Grid>
      </Box>
    </StyledPaperContainer>
  );
};

export { IndividualOrderResponsesItem };
