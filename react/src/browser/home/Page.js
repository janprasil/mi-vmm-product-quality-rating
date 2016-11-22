import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import { connect } from 'react-redux';
import { FileUpload } from 'redux-file-upload';
import { LineChart } from 'react-d3-components';

@connect(state => ({
  contours: state.api.getIn(['contours', 'data']),
  contoursPending: state.app.get('contoursPending'),
  uploadPending: state.app.get('uploadPending'),
}))
export default class Page extends Component {

  static propTypes = {
    contours: RPT.array,
    contoursPending: RPT.bool,
    uploadPending: RPT.bool
  }

  renderCharts() {
    const { contours } = this.props;
    if (!contours) return null;

    const data = contours.reduce((prev, x, idx) => {
      return [...prev, {
        label: `chart${idx}`,
        values: Object.keys(x.timeline).reduce((prev2, key) => {
          return [...prev2, { x: key, y: x.timeline[key] }];
        }, [])
      }];
    }, []);

    return (
      <LineChart
        data={data}
        width={2000}
        height={400}
      />
    );
  }

  render() {
    const { contoursPending, uploadPending } = this.props;
    return (
      <div>
        <div>
          <FileUpload
            allowedFileTypes={['jpg', 'jpeg']}
            data={{ type: 'picture' }}
            dropzoneId="fileUpload"
            multiple
            url="/webapi/contours"
          >
            <button>
              Klikněte nebo přetáhněte JPG soubory
            </button>
          </FileUpload>
        </div>
        {uploadPending && <p>Odesílám fotky...</p>}
        {contoursPending && <p>Zpracovávám grafy...</p>}
        {this.renderCharts()}
      </div>
    );
  }
}
