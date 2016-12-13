import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import { connect } from 'react-redux';
import { fetchReferences, deleteAllReference, startProcessing } from '../../common/api/actions';
import { FileUpload } from 'redux-file-upload';
import { LineChart } from 'react-d3-components';

@connect(state => ({
  references: state.api.getIn(['references', 'data']),
  referencesError: state.api.getIn(['references', 'error']),
  referencesPending: state.api.getIn(['references', 'pending']),
  dtw: state.api.getIn(['dtw', 'data']),
  dtwError: state.api.getIn(['dtw', 'error']),
  dtwPending: state.api.getIn(['dtw', 'pending']),
  uploadPending: state.app.get('uploadPending'),
}), { fetchReferences, deleteAllReference, startProcessing })
export default class Page extends Component {

  static propTypes = {
    references: RPT.array,
    referencesError: RPT.bool,
    referencesPending: RPT.bool,
    deleteAllReference: RPT.func,
    dtw: RPT.array,
    dtwError: RPT.bool,
    dtwPending: RPT.bool,
    fetchReferences: RPT.func.isRequired,
    uploadPending: RPT.bool
  }

  componentDidMount() {
    const { fetchReferences } = this.props;
    fetchReferences();
  }

  render() {
    const { references, referencesError, referencesPending, deleteAllReference, startProcessing, uploadPending } = this.props;

    return (
      <div>
        <div>
          <h1>Referenční obrázky</h1>
          {references && references.map(x => <img src={`/assets/${x.get('contourImageUrl')}`} />)}
          <button onClick={() => deleteAllReference()}>Smazat vše</button>
          <FileUpload
            allowedFileTypes={['jpg', 'jpeg', 'png']}
            data={{ type: 'picture' }}
            dropzoneId="fileUpload"
            multiple
            url="/webapi/reference"
          >
            <button>
              Klikněte nebo přetáhněte referenční obrázky (JPG)
            </button>
          </FileUpload>
        </div>
        {referencesError && <p>Došlo k chybě. Určitě běží API?</p>}
        {uploadPending && <p>Odesílám fotky...</p>}
        {referencesPending && <p>Stahuji data a zpracovávám grafy...</p>}

        <h1>Obrázky k porovnání</h1>

        <button onClick={() => startProcessing()}>Spustit výpočet</button>
      </div>
    );
  }
}
