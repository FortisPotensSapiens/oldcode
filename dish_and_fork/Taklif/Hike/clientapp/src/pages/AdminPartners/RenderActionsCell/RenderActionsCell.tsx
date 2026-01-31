import styled from '@emotion/styled';
import { GridRenderCellParams } from '@mui/x-data-grid';
import { Button } from 'antd';
import { FC, useEffect } from 'react';

import { usePostAdminBlockPartner } from '~/api/v1/usePostAdminBlockPartner';
import { usePostAdminUnblockPartner } from '~/api/v1/usePostAdminUnblockPartner ';
import { PartnerReadModel, PartnerState } from '~/types';

import { useClick } from './useClick';

export const UPDATED_ADMIN_PARTNERS_TABLE = 'UPDATED_ADMIN_PARTNERS_TABLE';

type RenderActionsCellProps = GridRenderCellParams<undefined, PartnerReadModel>;

const StyledButton = styled(Button)`
  display: block;
`;

const RenderActionsCell: FC<RenderActionsCellProps> = ({ row }) => {
  const [clickHandler, { isLoading, isSuccess }] = useClick(row);
  const blockMutation = usePostAdminBlockPartner();
  const unblockMutation = usePostAdminUnblockPartner();

  const handleBlock = () => {
    blockMutation.mutate({ sellerId: row.id });
  };

  const handleUnblock = () => {
    unblockMutation.mutate({ sellerId: row.id });
  };

  useEffect(() => {
    if (blockMutation.isSuccess) {
      window.postMessage(UPDATED_ADMIN_PARTNERS_TABLE);
    }
  }, [blockMutation.isSuccess]);

  useEffect(() => {
    if (unblockMutation.isSuccess) {
      window.postMessage(UPDATED_ADMIN_PARTNERS_TABLE);
    }
  }, [unblockMutation.isSuccess]);

  return (
    <>
      {!isSuccess && row.state === PartnerState.Created ? (
        <StyledButton disabled={isLoading} onClick={clickHandler}>
          Подтвердить
        </StyledButton>
      ) : undefined}
      {row.state !== PartnerState.Blocked ? (
        <StyledButton type="primary" onClick={handleBlock}>
          Заблокировать
        </StyledButton>
      ) : (
        <Button onClick={handleUnblock}>Разблокировать</Button>
      )}
    </>
  );
};

export { RenderActionsCell };
export type { RenderActionsCellProps };
