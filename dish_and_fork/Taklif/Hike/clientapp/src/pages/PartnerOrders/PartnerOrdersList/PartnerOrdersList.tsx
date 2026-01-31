import List from '@mui/material/List';

import { Row } from '../utils';
import PartnerOrdersListItem from './PartnerOrdersListItem';

const PartnerOrdersList = ({
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
  return (
    <>
      <List sx={{ bgcolor: 'background.paper' }}>
        {rows?.map((row) => {
          return (
            <PartnerOrdersListItem
              onReadyToDeliverClick={onReadyToDeliverClick}
              key={row.id}
              onRowClick={onRowClick}
              row={row}
            />
          );
        })}
      </List>
      {footer}
    </>
  );
};
export { PartnerOrdersList };
