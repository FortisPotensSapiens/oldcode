import { Box, Button, Divider, Grid, Paper, styled, Typography } from '@mui/material';
import AntdButton from 'antd/es/button';
import { format } from 'date-fns';
import { FC, SyntheticEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';

import { QuotedComment, ShowMoreTextTextField } from '~/components';
import { useCurrencySymbol } from '~/hooks';
import { generateIndividualOrderOfferPath, generatePartnerOrderInfoPath } from '~/routing';
import { OfferSellerReadModel } from '~/types';

const StyledSelectedLink = styled(AntdButton)`
  width: 100%;
  padding-top: 6px;
  padding-bottom: 6px;
  height: auto;
`;

type PartnerOffersListItemProps = {
  row: OfferSellerReadModel;
  onDelete: (offerId: string) => void;
};

const PartnerOffersListItem: FC<PartnerOffersListItemProps> = ({ row, onDelete }) => {
  const currency = useCurrencySymbol();

  const onDeleteCallback = () => {
    onDelete(row.id);
  };

  const navigate = useNavigate();

  const handleMoveToOrder = (e: SyntheticEvent) => {
    e.stopPropagation();
    e.preventDefault();
    navigate(generatePartnerOrderInfoPath(String(row.selectedOrderId)));
  };

  return (
    <Grid item xs={12}>
      <Paper>
        <Box p={2}>
          <Typography variant="h6">
            Заявка №{row.applictaion.number} {row.applictaion.title}
          </Typography>

          <Typography mt={2} variant="body1">
            Дата готовности: {format(new Date(row.date), 'dd.MM.yyyy')}
          </Typography>

          <Typography mt={2} variant="body1">
            Сумма: {row.sum} {currency}
          </Typography>

          {row.description && (
            <QuotedComment mt={2} py={1} variant="body2">
              <ShowMoreTextTextField>{row.description}</ShowMoreTextTextField>
            </QuotedComment>
          )}

          <Box mt={2}>
            <Button
              component={Link}
              fullWidth
              to={generateIndividualOrderOfferPath(row.applicationId, row.id)}
              variant="outlined"
            >
              Подробнее
            </Button>
          </Box>

          <Box mt={2}>
            {row.selectedOrderId ? (
              <StyledSelectedLink type="dashed" onClick={handleMoveToOrder}>
                Информация о заказе
              </StyledSelectedLink>
            ) : (
              <Button color="error" fullWidth variant="contained" onClick={onDeleteCallback}>
                Удалить
              </Button>
            )}
          </Box>
        </Box>

        <Divider />
      </Paper>
    </Grid>
  );
};
export default PartnerOffersListItem;
