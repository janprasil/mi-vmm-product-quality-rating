import React, { PureComponent as Component } from 'react';
import { FileUpload } from 'redux-file-upload';

export default class Page extends Component {

  render() {
    return (
      <div>
        <FileUpload
          allowedFileTypes={['jpg', 'jpeg']}
          data={{ type: 'picture' }}
          dropzoneId="fileUpload"
          url="/webapi/contours"
        >
          <button>
            Click or drag here
          </button>
        </FileUpload>
      </div>
    );
  }
}
