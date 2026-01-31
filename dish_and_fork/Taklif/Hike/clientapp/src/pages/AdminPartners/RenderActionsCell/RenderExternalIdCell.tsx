import { EditOutlined } from '@ant-design/icons';
import styled from '@emotion/styled';
import { GridRenderCellParams } from '@mui/x-data-grid';
import { Button } from 'antd';
import { FC, useCallback, useState } from 'react';

import { PartnerReadModel } from '~/types';

import { UPDATED_ADMIN_PARTNERS_TABLE } from './RenderActionsCell';
import { RenderExternalIdEditCell } from './RenderExternalIdEditCell';

type RenderExternalIdCellProps = GridRenderCellParams<undefined, PartnerReadModel>;

const StyledExternalId = styled('span')({
  marginRight: '0.5rem',
});

const RenderExternalIdCell: FC<RenderExternalIdCellProps> = ({ row }) => {
  const [isEdit, setIsEdit] = useState(false);

  const handleEditButton = useCallback(() => {
    setIsEdit(!isEdit);
  }, [isEdit, setIsEdit]);

  const handleEditClose = useCallback(() => {
    setIsEdit(false);
  }, [setIsEdit]);

  const handleSuccess = useCallback(() => {
    window.postMessage(UPDATED_ADMIN_PARTNERS_TABLE);
  }, []);

  return (
    <>
      {isEdit ? (
        <RenderExternalIdEditCell
          externalId={row.externalId}
          id={row.id}
          onClose={handleEditClose}
          onSuccess={handleSuccess}
        />
      ) : (
        <>
          {row.externalId ? <StyledExternalId>{row.externalId}</StyledExternalId> : ''}
          <Button type="ghost" icon={<EditOutlined />} onClick={handleEditButton} />
        </>
      )}
    </>
  );
};

export { RenderExternalIdCell };
