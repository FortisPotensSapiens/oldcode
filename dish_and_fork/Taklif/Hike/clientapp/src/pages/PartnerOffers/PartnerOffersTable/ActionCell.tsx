import { Button, Grid, styled } from '@mui/material';
import AntdButton from 'antd/es/button';
import { SyntheticEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';

import { generateIndividualOrderOfferPath, generatePartnerOrderInfoPath } from '~/routing';

import { ColumnProps } from '../types';

const StyledSelectedLink = styled(AntdButton)`
  width: 100%;
  padding-top: 6px;
  padding-bottom: 6px;
  height: auto;
`;

const ActionCell = ({ row, onDelete }: ColumnProps) => {
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
    <Grid container spacing={2}>
      <Grid item xs={12}>
        <Button
          fullWidth
          variant="outlined"
          component={Link}
          to={generateIndividualOrderOfferPath(row.applicationId, row.id)}
        >
          Подробнее
        </Button>
      </Grid>
      <Grid item xs={12}>
        {row.selectedOrderId ? (
          <StyledSelectedLink type="dashed" onClick={handleMoveToOrder}>
            Информация о заказе
          </StyledSelectedLink>
        ) : (
          <Button color="error" fullWidth variant="contained" onClick={onDeleteCallback}>
            Удалить
          </Button>
        )}
      </Grid>
    </Grid>
  );
};

export { ActionCell };
