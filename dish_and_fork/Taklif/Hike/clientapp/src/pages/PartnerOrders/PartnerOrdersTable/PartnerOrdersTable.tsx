import styled from '@emotion/styled';
import Paper from '@mui/material/Paper';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import { Button } from 'antd';
import { SyntheticEvent } from 'react';

import { getCurrencySymbol } from '~/utils';

import { getStyledStatus } from '../utils';
import PartnerOrdersTableItem from './PartnerOrdersTableItem';
import { Column, Row } from './types';

const StyleBottonContainer = styled.div`
  padding: 0.5rem 0;
`;

const PartnerOrdersTable = ({
  footer,
  onRowClick,
  onReadyToDeliverClick,
  rows,
}: {
  rows: Row[] | undefined;
  footer: JSX.Element;
  onRowClick: (orderId: string) => void;
  onReadyToDeliverClick: (orderId: string) => void;
}): JSX.Element => {
  const columns: Column[] = [
    { id: 'number', label: 'Номер заказа' },
    { id: 'buyerName', label: 'Покупатели' },
    {
      after: (_item: Row) => getCurrencySymbol('Rub'),
      id: 'summ',
      label: 'Сумма заказа',
    },
    {
      id: 'createDate',
      label: 'Дата создания',
    },
    {
      format: (value: Row) => {
        return getStyledStatus(value.status);
      },
      id: 'status',
      label: 'Статус',
    },
    {
      // eslint-disable-next-line react/no-unstable-nested-components
      format: (value: Row) => {
        const onClickCallback = (event: SyntheticEvent) => {
          event.stopPropagation();

          onReadyToDeliverClick(value.id);
        };

        const onClickTargetCallback = (event: SyntheticEvent) => {
          event.stopPropagation();

          window.open(value.sellerDeliveryTrackingUrl, '_blank')?.focus();
        };

        return (
          <StyleBottonContainer>
            {value.isReadyToDelivery ? <Button onClick={onClickCallback}>Отправить заказ</Button> : undefined}

            {value.sellerDeliveryTrackingUrl ? (
              <Button onClick={onClickTargetCallback}>Отследить доставку</Button>
            ) : undefined}
          </StyleBottonContainer>
        );
      },
      id: 'actions',
      label: '',
    },
  ];

  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 500 }}>
        <TableHead>
          <TableRow>
            {columns.map((column) => (
              <TableCell key={column.id}>{column.label}</TableCell>
            ))}
          </TableRow>
        </TableHead>
        <TableBody>
          {rows?.map((row) => {
            return <PartnerOrdersTableItem key={row.id} columns={columns} onRowClick={onRowClick} row={row} />;
          })}
        </TableBody>
      </Table>

      {footer}
    </TableContainer>
  );
};
export { PartnerOrdersTable };
