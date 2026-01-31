import { Button, Grid, Paper, styled, Typography } from '@mui/material';
import { Tooltip } from 'antd';
import AntdButton from 'antd/es/button';
import dayjs from 'dayjs';
import LocalizedFormat from 'dayjs/plugin/localizedFormat';
import timezone from 'dayjs/plugin/timezone';
import utc from 'dayjs/plugin/utc';
import { SyntheticEvent } from 'react';
import { useNavigate } from 'react-router-dom';

import { useOnOff } from '~/hooks';
import { SubmitCallback } from '~/pages/PartnerNewApps/NewAppItem';
import { FormDialog } from '~/pages/PartnerNewApps/NewAppItem/FormDialog';
import { generateIndividualOrderOfferPath, generateOrderPath } from '~/routing';
import { ApplicationDetailsReadModel, ApplicationReadModel } from '~/types';
import { getCurrencySymbol } from '~/utils';

dayjs.extend(LocalizedFormat);
dayjs.extend(utc);
dayjs.extend(timezone);

const StyledGrid = styled(Grid, {
  name: 'IndividualOrdersTableItem-StyledGrid',
  shouldForwardProp: (propName) => propName !== 'isHoverable',
})<{ isHoverable: boolean }>(({ theme, isHoverable }) => ({
  cursor: isHoverable ? 'pointer' : 'default',
  '&:hover': {
    boxShadow: theme.shadows[2],
  },
}));

const StyledDescription = styled(Typography)`
  opacity: 0.38;
  text-overflow: ellipsis;
  overflow: hidden;
`;

const StyledSelectedLink = styled(AntdButton)`
  width: 100%;
  padding-top: 6px;
  padding-bottom: 6px;
  height: auto;
`;

const TypographyWithOverflow = styled(Typography)`
  display: block;
  overflow: hidden;
  text-overflow: ellipsis;
`;

const IndividualOrdersTableItem = ({
  onClick,
  row,
  onComplete,
  showDescription = false,
  showLogin = false,
  showOfferButton = false,
  onDelete,
  isDeletable = false,
  hasOfferLink = false,
  hasOrderLink = false,
}: {
  row: ApplicationReadModel | ApplicationDetailsReadModel;
  onClick?: (rowId: string) => void;
  showDescription?: boolean;
  showLogin?: boolean;
  showOfferButton?: boolean;
  hasOfferLink?: boolean;
  onComplete?: SubmitCallback;
  isDeletable?: boolean;
  hasOrderLink?: boolean;
  onDelete?: (rowId: string) => void;
}) => {
  const onClickCallback = () => onClick?.(row.id);
  const navigate = useNavigate();

  const [visible, { off: hide, on: show }] = useOnOff();

  const onDeleteCallback = (e: SyntheticEvent) => {
    e.stopPropagation();

    onDelete && onDelete(row.id);
  };

  const handleMoveToOffer = (e: SyntheticEvent) => {
    e.stopPropagation();
    e.preventDefault();

    navigate(generateIndividualOrderOfferPath(row.id, String(row.selectedOfferId)));
  };
  const handleMoveToOrder = (e: SyntheticEvent) => {
    e.stopPropagation();
    e.preventDefault();
    navigate(generateOrderPath({ orderId: String(row.selectedOrderId) }));
  };

  return (
    <Paper>
      <StyledGrid container isHoverable={!!onClick} onClick={onClickCallback} padding={2} rowGap={1}>
        <Grid item xs={12}>
          <Grid container>
            <Grid item xs={9}>
              <Typography margin={0} padding={0} variant="body1">
                Заявка №{row.number}
              </Typography>
            </Grid>
            <Grid item textAlign="right" xs={3}>
              <Tooltip title={dayjs(row.created).local().format('L')}>
                <TypographyWithOverflow variant="body2">
                  {dayjs(row.created).local().format('L')}
                </TypographyWithOverflow>
              </Tooltip>
            </Grid>
          </Grid>
        </Grid>
        {showLogin && row && 'customer' in row ? (
          <Grid item xs={12}>
            <Grid container>
              <Grid item sm={9}>
                <Typography margin={0} padding={0} variant="body1">
                  Пользователь
                </Typography>
              </Grid>
              <Grid item textAlign="right" sm={3}>
                <Tooltip title={row.customer?.userName}>
                  <TypographyWithOverflow variant="body2">{row.customer?.userName}</TypographyWithOverflow>
                </Tooltip>
              </Grid>
            </Grid>
          </Grid>
        ) : undefined}
        <Grid item xs={12}>
          <Grid container>
            <Grid item xs={12}>
              <Typography overflow="hidden" textOverflow="ellipsis" variant="h6" whiteSpace="nowrap">
                {row.title}
              </Typography>
            </Grid>
          </Grid>
        </Grid>
        <Grid item xs={12}>
          <Grid container>
            <Grid item xs={12} sm={6}>
              <Typography style={{ opacity: 0.6 }} variant="body1">
                От&nbsp;{row.sumFrom}&nbsp;{getCurrencySymbol('Rub')} до&nbsp;{row.sumTo}&nbsp;
                {getCurrencySymbol('Rub')}
              </Typography>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Typography style={{ opacity: 0.4 }} variant="body2">
                c&nbsp;{dayjs(row.fromDate).local().format('L')} до&nbsp;{dayjs(row.toDate).local().format('L')}
              </Typography>
            </Grid>
            {row.description && showDescription ? (
              <Grid item marginTop={2} paddingTop={2} xs={12}>
                <StyledDescription variant="body1">{row.description}</StyledDescription>
              </Grid>
            ) : undefined}
          </Grid>
        </Grid>
        {showOfferButton ? (
          <Button color="primary" fullWidth onClick={show} variant="outlined">
            Откликнуться
          </Button>
        ) : undefined}
        {isDeletable ? (
          <Button color="error" fullWidth onClick={onDeleteCallback} variant="contained">
            Удалить
          </Button>
        ) : undefined}
        {hasOfferLink ? (
          <StyledSelectedLink type="dashed" onClick={handleMoveToOffer}>
            Информация об отклике
          </StyledSelectedLink>
        ) : undefined}
        {hasOrderLink ? (
          <StyledSelectedLink type="dashed" onClick={handleMoveToOrder}>
            Информация о заказе
          </StyledSelectedLink>
        ) : undefined}
      </StyledGrid>
      <FormDialog applicationId={row.id} onClose={hide} onComplete={onComplete} open={visible} />
    </Paper>
  );
};

export { IndividualOrdersTableItem };
